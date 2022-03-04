// --------------------------------------------------------------------------------------------------
// <copyright file="TraceService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using InmoIT.Modules.Accounting.Core.Abstractions;
using InmoIT.Modules.Accounting.Core.Entities;
using InmoIT.Modules.Accounting.Core.Exceptions;
using InmoIT.Shared.Core.Common.Enums;
using InmoIT.Shared.Core.Integration.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace InmoIT.Modules.Accounting.Infrastructure.Services
{
    public class TraceService : ITraceService
    {
        private readonly IAccountingDbContext _context;
        private readonly IStringLocalizer<TraceService> _localizer;
        private readonly ILogger<TraceService> _logger;

        public TraceService(
            IAccountingDbContext context,
            IStringLocalizer<TraceService> localizer,
            ILogger<TraceService> logger)
        {
            _context = context;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<int> GetCountRentAsync()
{
            var result = _context.PropertyTraces.Where(x => x.TransactionType == TransactionType.Rent);
            return await result.CountAsync();
        }

        public async Task<int> GetCountSalesAsync()
        {
            var result = _context.PropertyTraces.Where(x => x.TransactionType == TransactionType.Sale);
            return await result.CountAsync();
        }

        public async Task AddOrUpdateTrace(string codeInternal, string name, decimal price, decimal tax, string referenceNumber, Guid propertyId, TransactionType transactionType)
        {
            var propertyTransaction = new PropertyTransaction(propertyId, transactionType, referenceNumber);
            _logger.LogInformation(string.Format(_localizer["Added Transaction Reference:{0}:::Id:{1}"], propertyTransaction.ReferenceNumber, propertyTransaction.Id));
            await _context.PropertyTransactions.AddAsync(propertyTransaction);
            var propertyTrace = _context.PropertyTraces.FirstOrDefault(x => x.PropertyId == propertyId);

            if (propertyTrace != null)
            {
                propertyTrace.Name = name;
                propertyTrace.Value = price;
                propertyTrace.Tax = tax;
                propertyTrace.UpdatedOn = DateTime.Now;
                propertyTrace.TransactionType = transactionType;
                try
                {
                    _logger.LogInformation(string.Format(_localizer["Updated Trace Code:{0}:::Id:{1}"], propertyTrace.Code, propertyTrace.Id));
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
                    TransactionType = transactionType
                };
                try
                {
                    _logger.LogInformation(string.Format(_localizer["Added Trace Code:{0}:::Id:{1}"], propertyTrace.Code, propertyTrace.Id));
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