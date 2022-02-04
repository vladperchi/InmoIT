// --------------------------------------------------------------------------------------------------
// <copyright file="Property.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations.Schema;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Inmo.Core.Entities
{
    public class Property : BaseEntity
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public int SquareMeter { get; set; }

        public int NumberRooms { get; set; }

        public int NumberBathrooms { get; set; }

        public decimal SalePrice { get; set; }

        public decimal RentPrice { get; set; }

        public decimal SaleTax { get; set; }

        public decimal IncomeTax { get; set; }

        public string CodeInternal { get; set; }

        public int Year { get; set; }

        public bool HasParking { get; set; }

        public bool IsActive { get; set; }

        public Guid OwnerId { get; set; }

        public virtual Owner Owner { get; set; }

        public Guid PropertyTypeId { get; set; }

        public virtual PropertyType PropertyType { get; set; }

        [NotMapped]
        public decimal TolalSale => SalePrice + SaleTax;

        public decimal TotalRent => RentPrice + IncomeTax;

        public string PropertyTypeName => $"{PropertyType.Name}";

        public string OwnerName => $"{Owner.Name} {Owner.SurName}";
    }
}