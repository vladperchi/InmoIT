// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyRemovedEvent.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Inmo.Core.Features.Properties.Events
{
    public class PropertyRemovedEvent : Event
    {
        public Guid Id { get; }

        public PropertyRemovedEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
            RelatedEntities = new[] { typeof(Property) };
            EventDescription = "Removed Property.";
        }
    }
}