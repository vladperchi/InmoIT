﻿// --------------------------------------------------------------------------------------------------
// <copyright file="IApplicationDbContext.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using InmoIT.Shared.Core.Logging;
using InmoIT.Shared.Core.Entities;

namespace InmoIT.Shared.Core.Interfaces.Contexts
{
    public interface IApplicationDbContext : IDbContext
    {
        public DbSet<EventLog> EventLogs { get; set; }

        public DbSet<EntityReference> EntityReferences { get; set; }
    }
}