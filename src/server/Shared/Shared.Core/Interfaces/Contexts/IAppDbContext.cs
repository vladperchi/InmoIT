// --------------------------------------------------------------------------------------------------
// <copyright file="IAppDbContext.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Core.Entities;
using InmoIT.Shared.Core.Logging;
using Microsoft.EntityFrameworkCore;

namespace InmoIT.Shared.Core.Interfaces.Contexts
{
    public interface IApplicationDbContext : IDbContext
    {
        public DbSet<Logger> Loggers { get; set; }

        public DbSet<EntityReference> EntityReferences { get; set; }
    }
}