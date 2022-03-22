// --------------------------------------------------------------------------------------------------
// <copyright file="CorsPolicyExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System;
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Core.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace InmoIT.Shared.Infrastructure.Extensions
{
    public static class CorsPolicyExtensions
    {
        private const string CorsPolicy = nameof(CorsPolicy);

        internal static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            var corsSettings = services.GetOptions<CorsSettings>(nameof(CorsSettings));
            var originUrls = new List<string>();
            if (corsSettings.AngularUrl is not null)
                originUrls.AddRange(corsSettings.AngularUrl.Split('|', StringSplitOptions.RemoveEmptyEntries));
            if (corsSettings.BlazorUrl is not null)
                originUrls.AddRange(corsSettings.BlazorUrl.Split('|', StringSplitOptions.RemoveEmptyEntries));
            if (corsSettings.ReactUrl is not null)
                originUrls.AddRange(corsSettings.ReactUrl.Split('|', StringSplitOptions.RemoveEmptyEntries));
            if (corsSettings.VueUrl is not null)
                originUrls.AddRange(corsSettings.VueUrl.Split('|', StringSplitOptions.RemoveEmptyEntries));
            return services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(originUrls.ToArray());
                });
            });
        }

        internal static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) => app.UseCors(CorsPolicy);
    }
}