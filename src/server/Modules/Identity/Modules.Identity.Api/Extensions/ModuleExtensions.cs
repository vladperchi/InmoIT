// --------------------------------------------------------------------------------------------------
// <copyright file="ModuleExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Identity.Core.Extensions;
using InmoIT.Modules.Identity.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InmoIT.Modules.Identity.Api.Extensions
{
    public static class ModuleExtensions
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddIdentityCore()
                .AddIdentityInfrastructure(configuration);
            return services;
        }

        public static IApplicationBuilder UseIdentityModule(this IApplicationBuilder app)
        {
            return app;
        }
    }
}