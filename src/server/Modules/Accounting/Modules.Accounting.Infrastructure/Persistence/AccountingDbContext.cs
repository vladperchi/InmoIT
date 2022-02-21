// --------------------------------------------------------------------------------------------------
// <copyright file="AccountingDbContext.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Accounting.Core.Abstractions;
using InmoIT.Modules.Accounting.Core.Entities;
using InmoIT.Modules.Accounting.Infrastructure.Extensions;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Interfaces.Serialization.Serializer;
using InmoIT.Shared.Core.Logging;
using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InmoIT.Modules.Accounting.Infrastructure.Persistence
{
    public class AccountingDbContext : ModuleDbContext, IAccountingDbContext
    {
        private readonly PersistenceSettings _persistenceOptions;
        private readonly IJsonSerializer _json;

        protected override string Schema => SchemesConstant.Accounting;

        public AccountingDbContext(
            DbContextOptions<AccountingDbContext> options,
            IMediator mediator,
            IEventLogger eventLogger,
            IOptions<PersistenceSettings> persistenceOptions,
            IJsonSerializer json)
                : base(options, mediator, eventLogger, persistenceOptions, json)
        {
            _persistenceOptions = persistenceOptions.Value;
            _json = json;
        }

        public DbSet<PropertyTrace> PropertyTraces { get; set; }

        public DbSet<PropertyTransaction> PropertyTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyAccountingConfiguration(_persistenceOptions);
        }
    }
}