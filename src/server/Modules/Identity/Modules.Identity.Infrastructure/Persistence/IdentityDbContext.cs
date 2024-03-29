﻿// --------------------------------------------------------------------------------------------------
// <copyright file="IdentityDbContext.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Modules.Identity.Infrastructure.Extensions;
using InmoIT.Shared.Core.Domain;
using InmoIT.Shared.Core.Logging;
using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using InmoIT.Shared.Core.Interfaces.Contexts;
using InmoIT.Shared.Core.Interfaces.Serialization.Serializer;
using InmoIT.Shared.Core.Constants;

namespace InmoIT.Modules.Identity.Infrastructure.Persistence
{
    public sealed class IdentityDbContext : IdentityDbContext<InmoUser, InmoRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, InmoRoleClaim, IdentityUserToken<string>>,
        IIdentityDbContext,
        IModuleDbContext
    {
        private readonly IMediator _mediator;
        private readonly IEventLogger _eventLogger;
        private readonly PersistenceSettings _persistenceOptions;
        private readonly IJsonSerializer _json;

        internal string Schema => SchemesConstant.Identity;

        public IdentityDbContext(
            DbContextOptions<IdentityDbContext> options,
            IOptions<PersistenceSettings> persistenceOptions,
            IMediator mediator,
            IEventLogger eventLogger,
            IJsonSerializer json)
                : base(options)
        {
            _mediator = mediator;
            _eventLogger = eventLogger;
            _persistenceOptions = persistenceOptions.Value;
            _json = json;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.Ignore<Event>();
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.ApplyIdentityConfiguration(_persistenceOptions);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changes = OnBeforeSaveChanges();
            return await this.SaveChangeWithPublishEventsAsync(_eventLogger, _mediator, changes, _json, cancellationToken);
        }

        private List<(EntityEntry entityEntry, string oldValues, string newValues)> OnBeforeSaveChanges()
        {
            var result = new List<(EntityEntry entityEntry, string oldValues, string newValues)>();
            ChangeTracker.DetectChanges();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                var previousData = new Dictionary<string, object>();
                var currentData = new Dictionary<string, object>();
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    object originalValue = entry.GetDatabaseValues()?.GetValue<object>(propertyName);
                    switch (entry.State)
                    {
                        case EntityState.Unchanged:
                            break;
                        case EntityState.Added:
                            currentData[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            previousData[propertyName] = originalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified && originalValue?.Equals(property.CurrentValue) == false)
                            {
                                previousData[propertyName] = originalValue;
                                currentData[propertyName] = property.CurrentValue;
                            }

                            break;
                    }
                }

                string oldValues = previousData.Count == 0 ? null : _json.Serialize(previousData);
                string newValues = currentData.Count == 0 ? null : _json.Serialize(currentData);
                result.Add((entry, oldValues, newValues));
            }

            return result;
        }

        public override int SaveChanges()
        {
            var changes = OnBeforeSaveChanges();
            return this.SaveChangeWithPublishEvents(_eventLogger, _mediator, changes, _json);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return SaveChanges();
        }
    }
}