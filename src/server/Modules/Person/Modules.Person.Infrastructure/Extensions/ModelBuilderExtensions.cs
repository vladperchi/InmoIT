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

namespace InmoIT.Modules.Person.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyCustomerConfiguration(this ModelBuilder builder, PersistenceSettings persistenceOptions)
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
        }
    }
}