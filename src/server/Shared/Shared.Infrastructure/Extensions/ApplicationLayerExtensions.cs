// --------------------------------------------------------------------------------------------------
// <copyright file="ApplicationLayerExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Core.Integration.Application;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace InmoIT.Shared.Infrastructure.Extensions
{
    public static class ApplicationLayerExtensions
    {
        internal static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IEntityReferenceService, EntityReferenceService>();
            services.AddTransient<ILoggerService, LoggerService>();
            services.AddScoped<IJobService, HangfireService>();
            services.AddTransient<IUploadService, UploadService>();
            services.AddTransient<IMailService, SmtpMailService>();
            services.AddTransient<IMessageTwilioService, MessageTwilioService>();
            services.Configure<MailSettings>(config.GetSection(nameof(MailSettings)));
            services.Configure<CorsSettings>(config.GetSection(nameof(CorsSettings)));
            services.Configure<MessageTwilioSettings>(config.GetSection(nameof(MessageTwilioSettings)));
            services.Configure<SwaggerSettings>(config.GetSection(nameof(SwaggerSettings)));

            return services;
        }
    }
}