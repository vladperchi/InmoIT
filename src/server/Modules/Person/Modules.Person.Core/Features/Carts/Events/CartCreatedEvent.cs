// --------------------------------------------------------------------------------------------------
// <copyright file="CartCreatedEvent.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Person.Core.Features.Carts.Events
{
    public class CartCreatedEvent : Event
    {
        public Guid Id { get; }

        public Guid CustomerId { get; }

        public new DateTime Timestamp { get; }

        public CartCreatedEvent(Cart cart)
        {
           Id = cart.Id;
           CustomerId = cart.CustomerId;
           Timestamp = cart.Timestamp;
           AggregateId = cart.Id;
           RelatedEntities = new[] { typeof(Cart) };
           EventDescription = $"Created Cart Id {cart.Id} of Customer Id {cart.CustomerId}.";
        }
    }
}