// --------------------------------------------------------------------------------------------------
// <copyright file="AppDbContext.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Core.Entities;
using InmoIT.Shared.Core.Logging;
using InmoIT.Shared.Core.Interfaces.Contexts;
using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InmoIT.Shared.Infrastructure.Persistence
{
    internal class AppDbContext : DbContext, IAppDbContext
    {
        private readonly PersistenceSettings _persistenceOptions;

        protected string Schema => "Application";

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IOptions<PersistenceSettings> persistenceOptions)
                : base(options)
        {
            _persistenceOptions = persistenceOptions.Value;
        }

        public DbSet<EventLog> EventLogs { get; set; }

        public DbSet<EntityReference> EntityReferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyApplicationConfiguration(_persistenceOptions);
        }
    }
}