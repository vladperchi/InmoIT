// --------------------------------------------------------------------------------------------------
// <copyright file="IAccountingDbContext.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Accounting.Core.Entities;
using InmoIT.Shared.Core.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Accounting.Core.Abstractions
{
    public interface IAccountingDbContext : IDbContext
    {
        public DbSet<PropertyTrace> PropertyTraces { get; set; }

        public DbSet<PropertyTransaction> PropertyTransactions { get; set; }
    }
}