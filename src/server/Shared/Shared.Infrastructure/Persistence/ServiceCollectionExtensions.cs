// --------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Core.Settings;
using Hangfire;
using Hangfire.PostgreSql;
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
                // Default

                string connectionString = options.ConnectionStrings.MSSQL;
                services.AddMSSQL<T>(connectionString);
            }
            else if(options.UsePostgres)
            {
                string connectionString = options.ConnectionStrings.Postgres;
                services.AddPostgres<T>(connectionString);
            }
            else
            {
                string connectionString = options.ConnectionStrings.MySql;
                services.AddMySql<T>(connectionString);
            }

            return services;
        }

        private static IServiceCollection AddMSSQL<T>(this IServiceCollection services, string connectionString)
            where T : DbContext
        {
            services.AddDbContext<T>(m => m.UseSqlServer(connectionString, e => e.MigrationsAssembly(typeof(T).Assembly.FullName)));
            services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<T>();
            dbContext.Database.Migrate();

            return services;
        }

        private static IServiceCollection AddPostgres<T>(this IServiceCollection services, string connectionString)
            where T : DbContext
        {
            services.AddDbContext<T>(m => m.UseNpgsql(connectionString, e => e.MigrationsAssembly(typeof(T).Assembly.FullName)));
            services.AddHangfire(x => x.UsePostgreSqlStorage(connectionString));
            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<T>();
            dbContext.Database.Migrate();

            return services;
        }

        private static IServiceCollection AddMySql<T>(this IServiceCollection services, string connectionString)
            where T : DbContext
        {
            services.AddDbContext<T>(m => m.UseMySQL(connectionString, e => e.MigrationsAssembly(typeof(T).Assembly.FullName)));

            // Unsolved
            // Referencia: "Hangfire.MySqlStorage" Version="2.0.3"
            // services.AddHangfire(x => x.MySqlStorage(connectionString));

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<T>();
            dbContext.Database.Migrate();

            return services;
        }
    }
}