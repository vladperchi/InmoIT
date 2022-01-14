// --------------------------------------------------------------------------------------------------
// <copyright file="ICartService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Person.Carts;

namespace InmoIT.Shared.Core.Integration.Person
{
    public interface ICartService
    {
        Task<Result<GetCartByIdResponse>> GetDetailsCartAsync(Guid cartId);

        Task<Result<Guid>> RemoveCartAsync(Guid cartId);
    }
}