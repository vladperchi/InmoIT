// --------------------------------------------------------------------------------------------------
// <copyright file="CartItemRemovedEvent.cs" company="InmoIT">
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
    public class CartItemRemovedEvent : Event
    {
        public Guid Id { get; }

        public CartItemRemovedEvent(Guid id)
{
            Id = id;
            AggregateId = id;
            RelatedEntities = new[] { typeof(CartItem) };
            EventDescription = "Removed Item Cart.";
        }
    }
}