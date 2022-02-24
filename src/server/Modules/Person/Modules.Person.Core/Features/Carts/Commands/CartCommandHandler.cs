// --------------------------------------------------------------------------------------------------
// <copyright file="CartCommandHandler.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InmoIT.Modules.Person.Core.Abstractions;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Modules.Person.Core.Exceptions;
using InmoIT.Modules.Person.Core.Features.Carts.Events;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Features.Carts.Commands
{
    internal class CartCommandHandler :
        IRequestHandler<CreateCartCommand, Result<Guid>>,
        IRequestHandler<RemoveCartCommand, Result<Guid>>,
        IRequestHandler<ClearCartCommand, Result<Guid>>
    {
        private readonly ICustomerDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CartCommandHandler> _localizer;
        private readonly IDistributedCache _cache;

        public CartCommandHandler(
            ICustomerDbContext context,
            IMapper mapper,
            IStringLocalizer<CartCommandHandler> localizer,
            IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _cache = cache;
        }

        public async Task<Result<Guid>> Handle(CreateCartCommand command, CancellationToken cancellationToken)
        {
            if (!await _context.Customers
                .AnyAsync(x => x.Id == command.CustomerId, cancellationToken))
            {
                throw new CustomerNotFoundException(_localizer, command.CustomerId);
            }

            try
            {
                var cart = _mapper.Map<Cart>(command);
                cart.AddDomainEvent(new CartCreatedEvent(cart));
                await _context.Carts.AddAsync(cart, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<Guid>.SuccessAsync(cart.Id, _localizer["Cart Created"]);
            }
            catch (Exception)
            {
                throw new CartCustomException(_localizer, null);
            }
        }

        public async Task<Result<Guid>> Handle(RemoveCartCommand command, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);
            _ = cart ?? throw new CartNotFoundException(_localizer, command.Id);
            try
            {
                cart.AddDomainEvent(new CartRemovedEvent(cart.Id));
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync(cancellationToken);
                await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, Cart>(command.Id), cancellationToken);
                return await Result<Guid>.SuccessAsync(cart.Id, _localizer["Cart Deleted"]);
            }
            catch (Exception)
            {
                throw new CartCustomException(_localizer, null);
            }
        }

        public async Task<Result<Guid>> Handle(ClearCartCommand command, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);
            _ = cart ?? throw new CartNotFoundException(_localizer, command.Id);
            var cartItems = await _context.CartItems
                .Where(x => x.CartId == command.Id)
                .ToListAsync();
            if (cartItems.Count > 0)
            {
                _context.CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync(cancellationToken);
            }

            await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, Cart>(command.Id), cancellationToken);
            return await Result<Guid>.SuccessAsync(cart.Id, _localizer["Cart Cleared"]);
        }
    }
}