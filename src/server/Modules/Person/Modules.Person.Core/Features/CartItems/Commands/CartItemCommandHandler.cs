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
        private readonly IDistributedCache _cache;
        private readonly ICustomerDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CartItemCommandHandler> _localizer;

        public CartItemCommandHandler(
            IDistributedCache cache,
            ICustomerDbContext context,
            IMapper mapper,
            IStringLocalizer<CartItemCommandHandler> localizer)
        {
            _cache = cache;
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<Guid>> Handle(AddCartItemCommand command, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.Id == command.CartId, cancellationToken);

            if (cart == null)
            {
                throw new CartNotFoundException(_localizer);
            }

            if (cart.CartItems
                .Any(x => x.PropertyId == command.PropertyId))
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
                .Where(x => x.Id == command.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
            if (cartItem == null)
            {
                throw new CartItemNotFoundException(_localizer);
            }

            if (await _context.CartItems
                .AsNoTracking()
                .AnyAsync(
                    x => x.Id != command.Id
                      && x.CartId == command.CartId
                      && x.PropertyId == command.PropertyId, cancellationToken))
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
                .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);
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