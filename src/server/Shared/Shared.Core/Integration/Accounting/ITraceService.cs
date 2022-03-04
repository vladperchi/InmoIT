// --------------------------------------------------------------------------------------------------
// <copyright file="ITraceService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Common.Enums;

namespace InmoIT.Shared.Core.Integration.Accounting
{
    public interface ITraceService
    {
        public Task AddOrUpdateTrace(string codeInternal, string name, decimal value, decimal tax, string referenceNumber, Guid propertyId, TransactionType type);

        Task<int> GetCountSalesAsync();

        Task<int> GetCountRentAsync();
    }
}