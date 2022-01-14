// --------------------------------------------------------------------------------------------------
// <copyright file="CartItemCommandHandler.cs" company="InmoIT">
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
using InmoIT.Modules.Person.Core.Features.CartItems.Events;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Features.CartItems.Commands
{
    internal class CartItemCommandHandler :
        IRequestHandler<AddCartItemCommand, Result<Guid>>,
        IRequestHandler<UpdateCartItemCommand, Result<Guid>>,
        IRequestHandler<RemoveCartItemCommand, Result<Guid>>
    {
        private readonly ICustomerDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CartItemCommandHandler> _localizer;
        private readonly IDistributedCache _cache;

        public CartItemCommandHandler(
            ICustomerDbContext context,
            IMapper mapper,
            IStringLocalizer<CartItemCommandHandler> localizer,
            IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _cache = cache;
        }

        public async Task<Result<Guid>> Handle(AddCartItemCommand command, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(b => b.Id == command.CartId, cancellationToken);

            if (cart == null)
            {
                throw new CartNotFoundException(_localizer);
            }

            if (cart.CartItems.Any(i => i.PropertyId == command.PropertyId))
            {
                throw new CartItemAlreadyAddedException(_localizer);
            }

            try
            {
                var cartItem = _mapper.Map<CartItem>(command);
                cart.AddDomainEvent(new CartItemAddedEvent(cartItem));
                _context.CartItems.Add(cartItem);
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<Guid>.SuccessAsync(cartItem.Id, _localizer["Cart Item Saved"]);
            }
            catch (Exception)
            {
                throw new CartItemCustomException(_localizer, null);
            }
        }

        public async Task<Result<Guid>> Handle(UpdateCartItemCommand command, CancellationToken cancellationToken)
        {
            var cartItem = await _context.CartItems
                .Where(i => i.Id == command.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
            if (cartItem == null)
            {
                throw new CartItemNotFoundException(_localizer);
            }

            if (await _context.CartItems
                .AsNoTracking()
                .AnyAsync(
                    i => i.Id != command.Id
                      && i.CartId == command.CartId
                      && i.PropertyId == command.PropertyId, cancellationToken))
            {
                throw new CartItemAlreadyAddedException(_localizer);
            }

            try
            {
                cartItem = _mapper.Map<CartItem>(command);
                cartItem.AddDomainEvent(new CartItemUpdatedEvent(cartItem));
                _context.CartItems.Update(cartItem);
                await _context.SaveChangesAsync(cancellationToken);
                await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, CartItem>(command.Id), cancellationToken);
                return await Result<Guid>.SuccessAsync(cartItem.Id, _localizer["Cart Item Updated"]);
            }
            catch (Exception)
            {
                throw new CartItemCustomException(_localizer, null);
            }
        }

        public async Task<Result<Guid>> Handle(RemoveCartItemCommand command, CancellationToken cancellationToken)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(b => b.Id == command.Id, cancellationToken);
            if (cartItem == null)
            {
                throw new CartItemNotFoundException(_localizer);
            }

            try
            {
                cartItem.AddDomainEvent(new CartItemRemovedEvent(cartItem.Id));
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync(cancellationToken);
                await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, CartItem>(command.Id), cancellationToken);
                return await Result<Guid>.SuccessAsync(cartItem.Id, _localizer["Cart Item Deleted"]);
            }
            catch (Exception)
            {
                throw new CartItemCustomException(_localizer, null);
            }
        }
    }
}