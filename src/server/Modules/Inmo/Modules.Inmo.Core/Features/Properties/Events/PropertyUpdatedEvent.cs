// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyUpdatedEvent.cs" company="InmoIT">
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
    public class PropertyUpdatedEvent : Event
    {
        public Guid Id { get; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal Tax { get; set; }

        public string CodeInternal { get; set; }

        public int Year { get; set; }

        public bool IsActive { get; set; }

        public Guid OwnerId { get; set; }

        public PropertyUpdatedEvent(Property property)
        {
            Id = property.Id;
            Name = property.Name;
            Address = property.Address;
            Description = property.Description;
            Price = property.Price;
            Tax = property.Tax;
            CodeInternal = property.CodeInternal;
            Year = property.Year;
            IsActive = property.IsActive;
            OwnerId = property.OwnerId;
            AggregateId = property.Id;
            RelatedEntities = new[] { typeof(Property) };
            EventDescription = $"Updated Property {property.Name} with Id {property.Id}.";
        }
    }
}