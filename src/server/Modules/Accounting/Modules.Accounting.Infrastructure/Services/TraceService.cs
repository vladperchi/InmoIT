// --------------------------------------------------------------------------------------------------
// <copyright file="TraceService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using InmoIT.Modules.Accounting.Core.Abstractions;
using InmoIT.Modules.Accounting.Core.Entities;
using InmoIT.Modules.Accounting.Core.Exceptions;
using InmoIT.Shared.Core.Common.Enums;
using InmoIT.Shared.Core.Integration.Accounting;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Accounting.Infrastructure.Services
{
    public class TraceService : ITraceService
    {
        private readonly IAccountingDbContext _context;
        private readonly IStringLocalizer<TraceService> _localizer;

        public TraceService(
            IAccountingDbContext context,
            IStringLocalizer<TraceService> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task RecordTransaction(string codeInternal, string name, decimal price, decimal tax, string referenceNumber, Guid propertyId, TransactionType transactionType)
        {
            var propertyTransaction = new PropertyTransaction(propertyId, transactionType, referenceNumber);
            await _context.PropertyTransactions.AddAsync(propertyTransaction);
            var propertyTrace = _context.PropertyTraces.FirstOrDefault(x => x.PropertyId == propertyId);

            if (propertyTrace != null)
            {
                propertyTrace.Name = name;
                propertyTrace.Value = price;
                propertyTrace.Tax = tax;
                propertyTrace.UpdatedOn = DateTime.Now;
                propertyTrace.Type = transactionType;
                try
                {
                    _context.PropertyTraces.Update(propertyTrace);
                }
                catch (Exception)
                {
                    throw new TraceCustomException(_localizer, null);
                }
            }
            else
            {
                propertyTrace = new PropertyTrace()
                {
                    PropertyId = propertyId,
                    Code = codeInternal,
                    Name = name,
                    Value = price,
                    Tax = tax,
                    CreatedOn = DateTime.Now,
                    Type = transactionType
                };
                try
                {
                    _context.PropertyTraces.Add(propertyTrace);
                }
                catch (Exception)
                {
                    throw new TraceCustomException(_localizer, null);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}