// --------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Reflection;
using InmoIT.Modules.Inmo.Core.Abstractions;
using InmoIT.Modules.Inmo.Infrastructure.Persistence;
using InmoIT.Modules.Inmo.Infrastructure.Services;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Integration.Inmo;
using InmoIT.Shared.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace InmoIT.Modules.Inmo.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInmoInfrastructure(this IServiceCollection services)
        {
            services
                .AddDatabaseContext<InmoDbContext>()
                .AddScoped<IInmoDbContext>(provider => provider.GetService<InmoDbContext>());
            services.AddTransient<IDbSeeder, InmoDbSeeder>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IPropertyImageService, PropertyImageService>();
            services.AddTransient<IPropertyTypeService, PropertyTypeService>();
            services.AddTransient<IOwnerService, OwnerService>();
            return services;
        }

        public static IServiceCollection AddInmoValidation(this IServiceCollection services)
        {
            services.AddControllers().AddInmoValidation();
            return services;
        }
    }
}