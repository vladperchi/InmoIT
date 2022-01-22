// --------------------------------------------------------------------------------------------------
// <copyright file="CartItemAddedEvent.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Person.Core.Features.CartItems.Events
{
    public class CartItemAddedEvent : Event
    {
        public Guid Id { get; set; }

        public Guid CartId { get; }

        public Guid PropertyId { get; }

        public CartItemAddedEvent(CartItem cartItem)
        {
            Id = cartItem.Id;
            CartId = cartItem.CartId;
            PropertyId = cartItem.PropertyId;
            AggregateId = cartItem.Id;
            RelatedEntities = new[] { typeof(CartItem) };
            EventDescription = $"Added Item Id: {cartItem.PropertyId} to cart.";
        }
    }
}