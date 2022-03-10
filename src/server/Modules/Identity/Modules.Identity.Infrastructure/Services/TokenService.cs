// --------------------------------------------------------------------------------------------------
// <copyright file="TokenService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Modules.Identity.Core.Exceptions;
using InmoIT.Modules.Identity.Core.Settings;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Interfaces.Services.Identity;
using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Identity.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using InmoIT.Modules.Identity.Infrastructure.Extensions;

namespace InmoIT.Modules.Identity.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<InmoUser> _userManager;
        private readonly RoleManager<InmoRole> _roleManager;
        private readonly IStringLocalizer<TokenService> _localizer;
        private readonly ILogger<TokenService> _logger;
        private readonly SmsTwilioSettings _smsTwilioSettings;
        private readonly MailSettings _mailSettings;
        private readonly JwtSettings _config;
        private readonly ILoggerService _eventLog;
        private readonly ICurrentUser _currentUser;

        public TokenService(
            UserManager<InmoUser> userManager,
            RoleManager<InmoRole> roleManager,
            IOptions<JwtSettings> config,
            IStringLocalizer<TokenService> localizer,
            ILogger<TokenService> logger,
            IOptions<SmsTwilioSettings> smsTwilioSettings,
            IOptions<MailSettings> mailSettings,
            ILoggerService eventLog,
            ICurrentUser currentUser)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _localizer = localizer;
            _logger = logger;
            _smsTwilioSettings = smsTwilioSettings.Value;
            _mailSettings = mailSettings.Value;
            _config = config.Value;
            _eventLog = eventLog;
            _currentUser = currentUser;
        }

        public async Task<IResult<TokenUserResponse>> GetTokenAsync(TokenUserRequest request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email.Trim().Normalize());
            _ = user ?? throw new UserNotFoundException(_localizer, request.Email);
            if (!user.IsActive)
            {
                throw new UnauthorizedException(_localizer["User Not Active. Please contact the administrator."]);
            }

            if (_mailSettings.EnableVerification && !user.EmailConfirmed)
            {
                throw new UnauthorizedException(_localizer["E-Mail not confirmed. Please check your email account."]);
            }

            if (_smsTwilioSettings.EnableVerification && !user.PhoneNumberConfirmed)
            {
                throw new UnauthorizedException(_localizer["Phone Number not confirmed. Please check the sms on your mobile."]);
            }

            bool passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                throw new UnauthorizedException(_localizer["Invalid Credentials. Please contact the administrator"]);
            }

            return await GenerateTokensAndUpdateAsync(user, ipAddress);
        }

        public async Task<IResult<TokenUserResponse>> RefreshTokenAsync(RefreshTokenUserRequest request, string ipAddress)
        {
            if (request is null)
            {
                throw new UnauthorizedException(_localizer["Authentication Failed."]);
            }

            var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
            string userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            _ = user ?? throw new UserNotFoundException(_localizer, userEmail);
            if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new UnauthorizedException(_localizer["Invalid Client Token."]);
            }

            return await GenerateTokensAndUpdateAsync(user, ipAddress);
        }

        private async Task<IResult<TokenUserResponse>> GenerateTokensAndUpdateAsync(InmoUser user, string ipAddress)
        {
            if (_config.RefreshTokenExpirationInDays == 0)
            {
                throw new UnauthorizedException(_localizer["There is no RefreshTokenExpirationInDays value defined in the JwtSettings configuration."]);
            }

            string token = await GenerateJwtAsync(user, ipAddress);
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_config.RefreshTokenExpirationInDays);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var response = new TokenUserResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
                string shortToken = response.Token.Substring(response.Token.LastIndexOf("."));
                _logger.LogInformation($"User:{user.Email}::Id:{user.Id}::Token[..{shortToken}]Sensitive Information::Refresh:[{response.RefreshToken}]::ExpiryTime:[{response.RefreshTokenExpiryTime}]");
                return await Result<TokenUserResponse>.SuccessAsync(response);
            }
            else
            {
                await Result<string>.FailAsync(result.GetErrorMessages(_localizer));
                throw new IdentityException(_localizer["An error occurred while generating or updated Token"], result.GetErrorMessages(_localizer));
            }
        }

        private async Task<string> GenerateJwtAsync(InmoUser user, string ipAddress) =>
            GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user, ipAddress));

        private async Task<IEnumerable<Claim>> GetClaimsAsync(InmoUser user, string ipAddress)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            var permissionClaims = new List<Claim>();
            foreach (string role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
                var thisRole = await _roleManager.FindByNameAsync(role);
                var allPermissionsForThisRoles = await _roleManager.GetClaimsAsync(thisRole);
                permissionClaims.AddRange(allPermissionsForThisRoles);
            }

            return new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email),
                new("fullName", $"{user.FirstName} {user.LastName}"),
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new("ipAddress", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims)
            .Union(permissionClaims);
        }

        private string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            if (_config.TokenExpirationInMinutes == 0)
            {
                throw new UnauthorizedException(_localizer["There is no TokenExpirationInMinutes value defined in the JwtSettings configuration."]);
            }

            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(_config.TokenExpirationInMinutes),
               signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            if (string.IsNullOrEmpty(_config.Key))
            {
                throw new UnauthorizedException(_localizer["There is no key value defined in the JwtSettings configuration."]);
            }

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new UnauthorizedException(_localizer["Invalid Token."]);
            }

            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            if (string.IsNullOrEmpty(_config.Key))
            {
                throw new UnauthorizedException("There is no key value defined in the JwtSettings configuration.");
            }

            byte[] key = Encoding.UTF8.GetBytes(_config.Key);
            return new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        }
    }
}