// --------------------------------------------------------------------------------------------------
// <copyright file="ModuleExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Person.Core.Extensions;
using InmoIT.Modules.Person.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InmoIT.Modules.Person.Api.Extensions
{
    public static class ModuleExtensions
    {
        public static IServiceCollection AddCustomerModule(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCustomerCore()
                .AddCustomerInfrastructure()
                .AddCustomerValidation();
            return services;
        }

        public static IApplicationBuilder UseCustomerModule(this IApplicationBuilder app)
        {
            return app;
        }
    }
}