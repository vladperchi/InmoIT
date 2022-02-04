// --------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using InmoIT.Shared.Core.Logging;
using InmoIT.Shared.Core.Exceptions;
using InmoIT.Shared.Core.Interfaces.Contexts;
using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Infrastructure.Controllers;
using InmoIT.Shared.Infrastructure.Logging;
using InmoIT.Shared.Infrastructure.Interceptors;
using InmoIT.Shared.Infrastructure.Middlewares;
using InmoIT.Shared.Infrastructure.Persistence;
using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;

[assembly: InternalsVisibleTo("Api")]

namespace InmoIT.Shared.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddPersistenceSettings(config);
            services
                .AddDatabaseContext<ApplicationDbContext>()
                .AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<IEventLogger, EventLogger>();
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddControllers()
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
                })
                .AddMvcOptions(options =>
                {
                    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((value, propertyName) =>
                        throw new CustomException($"{propertyName}: value '{value}' is not valid.", statusCode: HttpStatusCode.BadRequest));
                });
            services.AddTransient<IValidatorInterceptor, ValidatorInterceptor>();
            services.AddApplicationLayer(config);
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddHangfireServer();
            services.AddSingleton<ExceptionHandlerMiddleware>();
            services.AddSwaggerDocumentation();
            services.AddCorsPolicy();
            services.AddApplicationSettings(config);
            services.AddDockerSettings(config);
            return services;
        }

        private static IServiceCollection AddPersistenceSettings(this IServiceCollection services, IConfiguration config)
        {
            return services
                .Configure<PersistenceSettings>(config.GetSection(nameof(PersistenceSettings)));
        }

        private static IServiceCollection AddApplicationSettings(this IServiceCollection services, IConfiguration config)
        {
            return services
                .Configure<ApplicationSettings>(config.GetSection(nameof(ApplicationSettings)));
        }

        private static IServiceCollection AddDockerSettings(this IServiceCollection services, IConfiguration config)
        {
            return services
                .Configure<DockerSettings>(config.GetSection(nameof(DockerSettings)));
        }
    }
}