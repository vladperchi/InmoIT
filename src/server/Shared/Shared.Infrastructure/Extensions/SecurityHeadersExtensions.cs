// --------------------------------------------------------------------------------------------------
// <copyright file="SecurityHeadersExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace InmoIT.Shared.Infrastructure.Extensions
{
    public static class SecurityHeadersExtensions
    {
        internal static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, IConfiguration config)
        {
            if (config.GetValue<bool>("HeaderSettings:Enable"))
            {
                var settings = GetHeaderSettings(config);
                app.Use(async (context, next) =>
                {
                    if (!string.IsNullOrWhiteSpace(settings.XFrameOptions))
                    {
                        context.Response.Headers.Add(HeadersConstant.XFRAMEOPTIONS, settings.XFrameOptions);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.XContentTypeOptions))
                    {
                        context.Response.Headers.Add(HeadersConstant.XCONTENTTYPEOPTIONS, settings.XContentTypeOptions);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.ReferrerPolicy))
                    {
                        context.Response.Headers.Add(HeadersConstant.REFERRERPOLICY, settings.ReferrerPolicy);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.PermissionsPolicy))
                    {
                        context.Response.Headers.Add(HeadersConstant.PERMISSIONSPOLICY, settings.PermissionsPolicy);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.SameSite))
                    {
                        context.Response.Headers.Add(HeadersConstant.SAMESITE, settings.SameSite);
                    }

                    if (!string.IsNullOrWhiteSpace(settings.XXSSProtection))
                    {
                        context.Response.Headers.Add(HeadersConstant.XXSSPROTECTION, settings.XXSSProtection);
                    }

                    await next();
                });
            }

            return app;
        }

        private static HeaderSettings GetHeaderSettings(IConfiguration config) =>
        config.GetSection(nameof(HeaderSettings)).Get<HeaderSettings>();
    }
}