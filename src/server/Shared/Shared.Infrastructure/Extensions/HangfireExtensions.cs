// --------------------------------------------------------------------------------------------------
// <copyright file="HangfireExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Infrastructure.HangfireJobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace InmoIT.Shared.Infrastructure.Extensions
{
    public static class HangfireExtensions
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HangfireExtensions));

        internal static IServiceCollection AddHangfireJobs(this IServiceCollection services, IConfiguration config)
        {
            services.AddHangfireServer(options => config.GetSection("HangfireSettings:Server").Bind(options));
            services.AddHangfireConsoleExtensions();

            var storageSettings = config.GetSection("HangfireSettings:Storage").Get<HangfireStorageSettings>();
            if (string.IsNullOrEmpty(storageSettings.StorageProvider))
            {
                throw new Exception("Hangfire Storage Provider is not configured.");
            }

            if (string.IsNullOrEmpty(storageSettings.ConnectionString))
            {
                throw new Exception("Hangfire ConnectionString is not configured.");
            }

            _logger.Information($"Hangfire Current Storage Provider : {storageSettings.StorageProvider.ToUpper()}");
            _logger.Information($"{storageSettings.Documentation}");

            services.AddSingleton<JobActivator, HangfireJobActivator>();
            services
                .AddHangfire((provider, hangfireConfig) => hangfireConfig
                .UseDatabase(storageSettings.StorageProvider, storageSettings.ConnectionString, config)
                .UseFilter(new HangfireJobFilter(provider))
                .UseFilter(new HangfireLogJobFilter())
                .UseConsole());

            return services;
        }

        private static IGlobalConfiguration UseDatabase(this IGlobalConfiguration hangfireConfig, string dataProvider, string connectionString, IConfiguration config) =>
            dataProvider.ToLowerInvariant() switch
            {
                DataProviderKeys.SqlServer =>
                    hangfireConfig.UseSqlServerStorage(connectionString, config.GetSection("HangfireSettings:Storage:Options").Get<SqlServerStorageOptions>()),
                _ => throw new Exception($"Hangfire Storage Provider {dataProvider} is not supported.")
            };

        internal static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration config)
        {
            var dashboardOptions = config.GetSection("HangfireSettings:Dashboard").Get<DashboardOptions>();
            dashboardOptions.DashboardTitle = config.GetSection("HangfireSettings:Dashboard:DashboardTitle").Value;
            if (config.GetValue<bool>("HangfireSettings:Authentication:EnablePermissions"))
            {
                dashboardOptions.Authorization = new[] { new HangfireAuthorizationFilter() };
            }
            else if (config.GetValue<bool>("HangfireSettings:Authentication:EnableBasic"))
            {
                dashboardOptions.Authorization = new[]
                {
                   new HangfireCustomBasicAuthenticationFilter
                   {
                        User = config.GetSection("HangfireSettings:Credentials:User").Value,
                        Pass = config.GetSection("HangfireSettings:Credentials:Password").Value
                   }
                };
            }

            return app.UseHangfireDashboard(config["HangfireSettings:Route"], dashboardOptions);
        }
    }
}