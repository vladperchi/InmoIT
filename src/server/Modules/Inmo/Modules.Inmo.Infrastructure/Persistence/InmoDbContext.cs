﻿// --------------------------------------------------------------------------------------------------
// <copyright file="InmoDbContext.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Inmo.Core.Abstractions;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Infrastructure.Extensions;
using InmoIT.Shared.Core.Logging;
using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using InmoIT.Shared.Core.Interfaces.Serialization.Serializer;
using InmoIT.Shared.Core.Constants;

namespace InmoIT.Modules.Inmo.Infrastructure.Persistence
{
    public sealed class InmoDbContext : ModuleDbContext, IInmoDbContext
    {
        private readonly PersistenceSettings _persistenceOptions;

        protected override string Schema => SchemesConstant.Inmo;

        public InmoDbContext(
            DbContextOptions<InmoDbContext> options,
            IMediator mediator,
            IEventLogger eventLogger,
            IOptions<PersistenceSettings> persistenceOptions,
            IJsonSerializer json)
                : base(options, mediator, eventLogger, persistenceOptions, json)
        {
            _persistenceOptions = persistenceOptions.Value;
        }

        public DbSet<Owner> Owners { get; set; }

        public DbSet<Property> Properties { get; set; }

        public DbSet<PropertyImage> PropertyImages { get; set; }

        public DbSet<PropertyType> PropertyTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyInmoConfiguration(_persistenceOptions);
        }
    }
}