// --------------------------------------------------------------------------------------------------
// <copyright file="JwtAuthenticationExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InmoIT.Modules.Identity.Core.Exceptions;
using InmoIT.Modules.Identity.Core.Settings;
using InmoIT.Shared.Core.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace InmoIT.Modules.Identity.Infrastructure.Extensions
{
    public static class JwtAuthenticationExtensions
    {
        internal static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = services.GetOptions<JwtSettings>(nameof(JwtSettings));
            byte[] key = Encoding.ASCII.GetBytes(jwtSettings.Key);
            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.Zero
                    };
                    bearer.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = x =>
                        {
                            if (x.Exception is SecurityTokenExpiredException)
                            {
                                throw new UnauthorizedException("The Token is expired.");
                            }
                            else
                            {
                                throw new IdentityException("An unhandled error has occurred.");
                            }
                        },
                        OnChallenge = x =>
                        {
                            x.HandleResponse();
                            if (!x.Response.HasStarted)
                            {
                                throw new UnauthorizedException("You are not Authorized.");
                            }

                            return Task.CompletedTask;
                        },
                        OnForbidden = _ =>
                        {
                            throw new ForbiddenException("You are not authorized to access this resource.");
                        }
                    };
                });
            return services;
        }
    }
}