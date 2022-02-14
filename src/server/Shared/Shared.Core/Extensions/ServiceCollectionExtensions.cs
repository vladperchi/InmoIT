// --------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Linq;
using System.Reflection;
using InmoIT.Shared.Core.Behaviors;
using InmoIT.Shared.Core.Features.Queries.Validators;
using InmoIT.Shared.Core.Interfaces.Serialization;
using InmoIT.Shared.Core.Serialization;
using InmoIT.Shared.Core.Settings;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InmoIT.Shared.Core.Interfaces.Serialization.Serializer;

namespace InmoIT.Shared.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedApplication(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CacheSettings>(config.GetSection(nameof(CacheSettings)));
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }

        public static IServiceCollection AddSerialization(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<SerializationSettings>(config.GetSection(nameof(SerializationSettings)));
            var options = services.GetOptions<SerializationSettings>(nameof(SerializationSettings));
            services.AddSingleton<IJsonSerializerSettingsOptions, JsonSerializerSettingsOptions>();
            if (options.UseSystemTextJson)
            {
                services
                    .AddSingleton<IJsonSerializer, SystemTextJsonSerializer>()
                    .Configure<JsonSerializerSettingsOptions>(configureOptions =>
                    {
                        if (!configureOptions.JsonSerializerOptions.Converters.Any(c => c.GetType() == typeof(TimespanJsonConverter)))
                        {
                            configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                        }
                    });
            }
            else if (options.UseNewtonsoftJson)
            {
                services
                    .AddSingleton<IJsonSerializer, NewtonSoftJsonSerializer>();
            }

            return services;
        }

        public static T GetOptions<T>(this IServiceCollection services, string sectionName)
            where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);

            return options;
        }

        public static IServiceCollection AddPaginatedFilterValidatorsFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var validatorTypes = assembly.GetExportedTypes().Where(t => t.IsClass && !t.IsAbstract && t.BaseType?.IsGenericType ==
            true).Select(t => new
                {
                    BaseGenericType = t.BaseType,
                    CurrentType = t
                }).Where(t => t.BaseGenericType?.GetGenericTypeDefinition() == typeof(PaginatedFilterValidator<,,>)).ToList();

            foreach (var validatorType in validatorTypes)
            {
                var validatorTypeGenericArguments = validatorType.BaseGenericType.GetGenericArguments().ToList();
                var validatorServiceType = typeof(IValidator<>).MakeGenericType(validatorTypeGenericArguments.Last());
                services.AddScoped(validatorServiceType, validatorType.CurrentType);
            }

            return services;
        }
    }
}