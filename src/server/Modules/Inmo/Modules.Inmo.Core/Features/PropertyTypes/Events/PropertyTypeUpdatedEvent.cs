// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyTypeUpdatedEvent.cs" company="InmoIT">
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
    public class PropertyTypeUpdatedEvent : Event
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public PropertyTypeUpdatedEvent(PropertyType propertyType)
        {
            Id = propertyType.Id;
            Name = propertyType.Name;
            Description = propertyType.Description;
            ImageUrl = propertyType.ImageUrl;
            IsActive = propertyType.IsActive;
            AggregateId = propertyType.Id;
            RelatedEntities = new[] { typeof(Owner) };
            EventDescription = $"Updated Property Type:{propertyType.Name}:::Id:{propertyType.Id}";
        }
    }
}