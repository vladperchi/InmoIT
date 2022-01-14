// --------------------------------------------------------------------------------------------------
// <copyright file="CartService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Person.Core.Features.Carts.Commands;
using InmoIT.Modules.Person.Core.Features.Carts.Queries;
using InmoIT.Shared.Core.Integration.Person;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Person.Carts;
using MediatR;

namespace InmoIT.Modules.Person.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly IMediator _mediator;

        public CartService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<GetCartByIdResponse>> GetDetailsCartAsync(Guid cartId)
        {
            return await _mediator.Send(new GetCartByIdQuery(cartId, true));
        }

        public async Task<Result<Guid>> RemoveCartAsync(Guid cartId)
        {
            return await _mediator.Send(new RemoveCartCommand(cartId));
        }
    }
}