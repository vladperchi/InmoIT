// --------------------------------------------------------------------------------------------------
// <copyright file="ModuleExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Accounting.Core.Extensions;
using InmoIT.Modules.Accounting.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InmoIT.Modules.Accounting.Api.Extensions
{
    public static class ModuleExtensions
    {
        public static IServiceCollection AddAccountingModule(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAccountingCore()
                .AddAccountingInfrastructure();
            return services;
        }

        public static IApplicationBuilder UseAccountingModule(this IApplicationBuilder app)
        {
            return app;
        }
    }
}