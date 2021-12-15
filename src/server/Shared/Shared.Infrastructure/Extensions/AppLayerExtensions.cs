// --------------------------------------------------------------------------------------------------
// <copyright file="AppLayerExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using InmoIT.Shared.Core.Integration.Application;
using InmoIT.Shared.Infrastructure.Services;

namespace InmoIT.Shared.Infrastructure.Extensions
{
    public static class AppLayerExtensions
    {
        internal static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddTransient<ILoggerService, LoggerService>()
                .AddTransient<IUploadService, UploadService>()
                .AddTransient<IMailService, SmtpMailService>()
                .AddTransient<IMessageService, MessageService>()
                .AddTransient<IEntityReferenceService, EntityReferenceService>();
            services
                .Configure<MailSettings>(config.GetSection(nameof(MailSettings)))
                .Configure<CorsSettings>(config.GetSection(nameof(CorsSettings)))
                .Configure<MessageSettings>(config.GetSection(nameof(MessageSettings)))
                .Configure<SwaggerSettings>(config.GetSection(nameof(SwaggerSettings)));
            services
                .AddScoped<IJobService, HangfireService>();

            return services;
        }
    }
}