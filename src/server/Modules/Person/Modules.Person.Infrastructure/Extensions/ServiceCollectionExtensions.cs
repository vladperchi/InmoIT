// --------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Person.Core.Abstractions;
using InmoIT.Modules.Person.Infrastructure.Persistence;
using InmoIT.Modules.Person.Infrastructure.Services;
using InmoIT.Shared.Core.Integration.Person;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace InmoIT.Modules.Person.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomerInfrastructure(this IServiceCollection services)
        {
            services
                 .AddDatabaseContext<CustomerDbContext>()
                 .AddScoped<ICustomerDbContext>(provider => provider.GetService<CustomerDbContext>());
            services.AddTransient<IDbSeeder, CustomerDbSeeder>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<ICustomerService, CustomerService>();
            return services;
        }

        public static IServiceCollection AddCustomerValidation(this IServiceCollection services)
        {
            services.AddControllers().AddCustomerValidation();
            return services;
        }
    }
}