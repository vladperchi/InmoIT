// --------------------------------------------------------------------------------------------------
// <copyright file="IInmoDbContext.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Inmo.Core.Abstractions
{
    public interface IInmoDbContext : IDbContext
    {
        public DbSet<Owner> Owners { get; set; }

        public DbSet<Property> Properties { get; set; }

        public DbSet<PropertyImage> PropertyImages { get; set; }

        public DbSet<PropertyTrace> PropertyTraces { get; set; }

        public DbSet<PropertyDocument> PropertyDocuments { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }
    }
}