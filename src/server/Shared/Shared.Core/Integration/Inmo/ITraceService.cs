// --------------------------------------------------------------------------------------------------
// <copyright file="ITraceService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.Traces;

namespace InmoIT.Shared.Core.Integration.Inmo
{
    public interface ITraceService
    {
        Task<Result<GetPropertyTraceByIdResponse>> GetPropertyTraceDetailsAsync(Guid traceId);

        public Task RecordTrace(Guid Id, string DateSale, string Name, decimal Value, decimal Tax, Guid PropertyId, string referenceNumber, bool isSale = true);
    }
}