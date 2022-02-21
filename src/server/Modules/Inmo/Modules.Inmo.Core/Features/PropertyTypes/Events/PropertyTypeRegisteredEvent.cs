// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyTypeRegisteredEvent.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;

using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Events
{
    public class PropertyTypeRegisteredEvent : Event
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public PropertyTypeRegisteredEvent(PropertyType propertyType)
        {
            Id = propertyType.Id;
            Name = propertyType.Name;
            Description = propertyType.Description;
            ImageUrl = propertyType.ImageUrl;
            IsActive = propertyType.IsActive;
            AggregateId = propertyType.Id;
            RelatedEntities = new[] { typeof(Owner) };
            EventDescription = $"Registered Property Type Name: {propertyType.Name} with Id {propertyType.Id}.";
        }
    }
}