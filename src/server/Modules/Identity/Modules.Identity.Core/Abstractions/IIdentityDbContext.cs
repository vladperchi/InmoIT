// --------------------------------------------------------------------------------------------------
// <copyright file="IIdentityDbContext.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Shared.Core.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Identity.Core.Abstractions
{
    public interface IIdentityDbContext : IDbContext
    {
        public DbSet<InmoUser> Users { get; set; }

        public DbSet<InmoRole> Roles { get; set; }

        public DbSet<InmoRoleClaim> RoleClaims { get; set; }
    }
}