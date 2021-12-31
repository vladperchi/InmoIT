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
using System.Net;
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

using Twilio.Http;

namespace InmoIT.Modules.Identity.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<InmoUser> _userManager;
        private readonly RoleManager<InmoRole> _roleManager;
        private readonly IStringLocalizer<TokenService> _localizer;
        private readonly SmsTwilioSettings _smsTwilioSettings;
        private readonly MailSettings _mailSettings;
        private readonly JwtSettings _config;
        private readonly ILoggerService _eventLog;

        public TokenService(
            UserManager<InmoUser> userManager,
            RoleManager<InmoRole> roleManager,
            IOptions<JwtSettings> config,
            IStringLocalizer<TokenService> localizer,
            IOptions<SmsTwilioSettings> smsTwilioSettings,
            IOptions<MailSettings> mailSettings,
            ILoggerService eventLog)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _localizer = localizer;
            _smsTwilioSettings = smsTwilioSettings.Value;
            _mailSettings = mailSettings.Value;
            _config = config.Value;
            _eventLog = eventLog;
        }

        public async Task<IResult<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new IdentityException(_localizer["User Not Found."], statusCode: HttpStatusCode.Unauthorized);
            }

            if (!user.IsActive)
            {
                throw new IdentityException(_localizer["User Not Active. Please contact the administrator."], statusCode: HttpStatusCode.Unauthorized);
            }

            if (_mailSettings.EnableVerification && !user.EmailConfirmed)
            {
                throw new IdentityException(_localizer["E-Mail not confirmed."], statusCode: HttpStatusCode.Unauthorized);
            }

            if (_smsTwilioSettings.EnableVerification && !user.PhoneNumberConfirmed)
            {
                throw new IdentityException(_localizer["Phone Number not confirmed."], statusCode: HttpStatusCode.Unauthorized);
            }

            bool passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                throw new IdentityException(_localizer["Invalid Credentials."], statusCode: HttpStatusCode.Unauthorized);
            }

            try
            {
                user.RefreshToken = GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_config.RefreshTokenExpirationInDays);
                await _userManager.UpdateAsync(user);
                string token = await GenerateJwtAsync(user, ipAddress);
                var response = new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
                await _eventLog.LogCustomEventAsync(new() { Description = $"Generated Tokens for {user.Email}.", Email = user.Email });
                return await Result<TokenResponse>.SuccessAsync(response);
            }
            catch (Exception)
            {
                throw new IdentityCustomException(_localizer, null);
            }
        }

        public async Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
        {
            if (request is null)
            {
                throw new IdentityException(_localizer["Invalid Client Token."], statusCode: HttpStatusCode.Unauthorized);
            }

            var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
            string userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new IdentityException(_localizer["User Not Found."], statusCode: HttpStatusCode.NotFound);
            }

            if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new IdentityException(_localizer["Invalid Client Token."], statusCode: HttpStatusCode.Unauthorized);
            }

            try
            {
                string token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user, ipAddress));
                user.RefreshToken = GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_config.RefreshTokenExpirationInDays);
                await _userManager.UpdateAsync(user);
                var response = new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
                return await Result<TokenResponse>.SuccessAsync(response);
            }
            catch (Exception)
            {
                throw new IdentityCustomException(_localizer, null);
            }
        }

        private async Task<string> GenerateJwtAsync(InmoUser user, string ipAddress)
        {
            return GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user, ipAddress));
        }

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
            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(_config.TokenExpirationInMinutes),
               signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
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
                throw new IdentityException(_localizer["Invalid Token."], statusCode: HttpStatusCode.Unauthorized);
            }

            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] secret = Encoding.UTF8.GetBytes(_config.Key);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }
    }
}