// --------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Reflection;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Modules.Identity.Core.Settings;
using InmoIT.Modules.Identity.Infrastructure.Persistence;
using InmoIT.Modules.Identity.Infrastructure.Services;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Interfaces.Services.Identity;
using InmoIT.Shared.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InmoIT.Modules.Identity.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services
                .AddHttpContextAccessor()
                .AddScoped<ICurrentUser, CurrentUser>()
                .Configure<JwtSettings>(configuration.GetSection("JwtSettings"))
                .AddTransient<ITokenService, TokenService>()
                .AddTransient<IIdentityService, IdentityService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IRoleService, RoleService>()
                .AddTransient<IRoleClaimService, RoleClaimService>()
                .AddDatabaseContext<IdentityDbContext>()
                .AddScoped<IIdentityDbContext>(provider => provider.GetService<IdentityDbContext>())
                .AddIdentity<InmoUser, InmoRole>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();
            services.AddTransient<IDbSeeder, IdentityDbSeeder>();
            services.AddPermissions(configuration);
            services.AddJwtAuthentication(configuration);
            return services;
        }
    }
}