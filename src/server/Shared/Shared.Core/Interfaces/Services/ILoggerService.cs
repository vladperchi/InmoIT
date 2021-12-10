// --------------------------------------------------------------------------------------------------
// <copyright file="ILoggerService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using InmoIT.Shared.Core.Logging;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Identity.Logging;

namespace InmoIT.Shared.Core.Interfaces.Services
{
    public interface ILoggerService
    {
        Task<PaginatedResult<Logger>> GetAllAsync(GetAllLogsRequest request);

        Task<Result<string>> LogCustomEventAsync(LogRequest request);
    }
}