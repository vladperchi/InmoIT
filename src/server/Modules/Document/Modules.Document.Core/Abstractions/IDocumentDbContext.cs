// --------------------------------------------------------------------------------------------------
// <copyright file="IDocumentDbContext.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Document.Core.Entities;
using InmoIT.Shared.Core.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Document.Core.Abstractions
{
    public interface IDocumentDbContext : IDbContext
    {
        public DbSet<Entities.Document> Documents { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }
    }
}