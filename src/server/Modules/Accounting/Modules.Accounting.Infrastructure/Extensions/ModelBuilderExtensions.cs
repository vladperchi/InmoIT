﻿// --------------------------------------------------------------------------------------------------
// <copyright file="ModelBuilderExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Linq;
using InmoIT.Modules.Accounting.Core.Entities;
using InmoIT.Shared.Core.Settings;
using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Accounting.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyAccountingConfiguration(this ModelBuilder builder, PersistenceSettings persistenceOptions)
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

            builder.Entity<PropertyTrace>(entity =>
            {
                entity.ToTable(name: "PropertyTraces");

                if (persistenceOptions.UseMsSql)
                {
                    entity.Property(p => p.Value)
                        .HasColumnType("decimal(23, 2)");
                    entity.Property(p => p.Tax)
                        .HasColumnType("decimal(23, 2)");
                }
            });
        }
    }
}