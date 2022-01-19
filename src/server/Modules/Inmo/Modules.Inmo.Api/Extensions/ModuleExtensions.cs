// --------------------------------------------------------------------------------------------------
// <copyright file="ModuleExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Inmo.Core.Extensions;
using InmoIT.Modules.Inmo.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InmoIT.Modules.Inmo.Api.Extensions
{
    public static class ModuleExtensions
    {
        public static IServiceCollection AddInmoModule(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddInmoCore()
                .AddInmoInfrastructure()
                .AddInmoValidation();
            return services;
        }

        public static IApplicationBuilder UseInmoModule(this IApplicationBuilder app)
        {
            return app;
        }
    }
}