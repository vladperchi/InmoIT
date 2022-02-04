// --------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Net;
using Hangfire;
using InmoIT.Shared.Core.Exceptions;
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Core.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InmoIT.Shared.Infrastructure.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseContext<T>(this IServiceCollection services)
            where T : DbContext
        {
            var options = services.GetOptions<PersistenceSettings>(nameof(PersistenceSettings));
            if (options.UseMsSql)
            {
                string connectionString = options.ConnectionStrings.MSSQL;
                services.AddMSSQL<T>(connectionString);
            }

            return services;
        }

        private static IServiceCollection AddMSSQL<T>(this IServiceCollection services, string connectionString)
            where T : DbContext
        {
            try
            {
                services.AddDbContext<T>(m => m.UseSqlServer(connectionString, e => e.MigrationsAssembly(typeof(T).Assembly.FullName)));
                services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<T>();
                dbContext.Database.Migrate();
                return services;
            }
            catch (Exception)
            {
                throw new CustomException("Migration errors have occurred..", statusCode: HttpStatusCode.BadRequest);
            }
        }
    }
}