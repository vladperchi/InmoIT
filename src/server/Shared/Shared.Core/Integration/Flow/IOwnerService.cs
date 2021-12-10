// --------------------------------------------------------------------------------------------------
// <copyright file="IOwnerService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Flow.Owners;

namespace InmoIT.Shared.Core.Integration.Flow
{
    public interface IOwnerService
    {
        Task<Result<GetOwnerByIdResponse>> GetDetailsAsync(Guid ownerId);
    }
}