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

        public int SquareMeter { get; set; }

        public int NumberRooms { get; set; }

        public int NumberBathrooms { get; set; }

        public decimal SalePrice { get; set; }

        public decimal RentalPrice { get; set; }

        public decimal SalesTax { get; set; }

        public decimal IncomeTax { get; set; }

        public string CodeInternal { get; set; }

        public int Year { get; set; }

        public bool HasParking { get; set; }

        public bool IsActive { get; set; }

        public Guid OwnerId { get; set; }

        public Guid PropertyTypeId { get; set; }

        public PropertyUpdatedEvent(Property property)
        {
            Id = property.Id;
            Name = property.Name;
            Address = property.Address;
            Description = property.Description;
            SquareMeter = property.SquareMeter;
            NumberRooms = property.NumberRooms;
            NumberBathrooms = property.NumberBathrooms;
            SalePrice = property.SalePrice;
            SalesTax = property.SaleTax;
            RentalPrice = property.RentPrice;
            IncomeTax = property.IncomeTax;
            CodeInternal = property.CodeInternal;
            Year = property.Year;
            HasParking = property.HasParking;
            IsActive = property.IsActive;
            OwnerId = property.OwnerId;
            PropertyTypeId = property.PropertyTypeId;
            AggregateId = property.Id;
            RelatedEntities = new[] { typeof(Property) };
            EventDescription = $"Updated Property Name: {property.Name} with Id: {property.Id}.";
        }
    }
}