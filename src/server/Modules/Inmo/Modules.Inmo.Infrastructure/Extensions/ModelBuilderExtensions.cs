// --------------------------------------------------------------------------------------------------
// <copyright file="ModelBuilderExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Linq;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Settings;
using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Inmo.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyInmoConfiguration(this ModelBuilder builder, PersistenceSettings persistenceOptions)
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

            builder.Entity<Property>(entity =>
            {
                entity.ToTable(name: "Properties");

                if (persistenceOptions.UseMsSql)
                {
                    entity.Property(p => p.Price)
                        .HasColumnType("decimal(23, 2)");
                }
            });
        }
    }
}