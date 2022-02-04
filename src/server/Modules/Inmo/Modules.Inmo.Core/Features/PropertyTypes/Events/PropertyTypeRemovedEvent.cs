// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyTypeRemovedEvent.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Events
{
    public class PropertyTypeRemovedEvent : Event
    {
        public Guid Id { get; }

        public PropertyTypeRemovedEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
            RelatedEntities = new[] { typeof(PropertyType) };
            EventDescription = "Deleted Property Type.";
        }
    }
}