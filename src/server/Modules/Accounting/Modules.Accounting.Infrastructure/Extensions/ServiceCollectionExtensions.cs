// --------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Accounting.Core.Abstractions;
using InmoIT.Modules.Accounting.Infrastructure.Persistence;
using InmoIT.Modules.Accounting.Infrastructure.Services;
using InmoIT.Shared.Core.Integration.Accounting;
using InmoIT.Shared.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace InmoIT.Modules.Accounting.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAccountingInfrastructure(this IServiceCollection services)
        {
            services
                 .AddDatabaseContext<AccountingDbContext>()
                 .AddScoped<IAccountingDbContext>(provider => provider.GetService<AccountingDbContext>());
            services.AddTransient<ITraceService, TraceService>();
            return services;
        }
    }
}