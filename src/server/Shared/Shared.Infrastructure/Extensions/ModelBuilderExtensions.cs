// --------------------------------------------------------------------------------------------------
// <copyright file="ModelBuilderExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Linq;
using InmoIT.Shared.Core.Settings;
using Microsoft.EntityFrameworkCore;

namespace InmoIT.Shared.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyApplicationConfiguration(this ModelBuilder builder, PersistenceSettings persistenceOptions)
        {
            if (persistenceOptions.UseMsSql)
            {
                foreach (var property in builder.Model.GetEntityTypes()
                    .SelectMany(t => t.GetProperties())
                    .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
                {
                    property.SetColumnType("decimal(23,2)");
                }
            }
            else if (persistenceOptions.UsePostgres)
            {
                // Build model for Postgres
            }
            else if (persistenceOptions.UseMySql)
            {
                // Build model for MySql
            }
        }

        public static void ApplyModuleConfiguration(this ModelBuilder builder, PersistenceSettings persistenceOptions)
        {
            // Build model for MSSQL, Postgres and MySql
        }
    }
}