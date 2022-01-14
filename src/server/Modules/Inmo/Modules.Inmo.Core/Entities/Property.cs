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

        public decimal Price { get; set; }

        public decimal Tax { get; set; }

        public string CodeInternal { get; set; }

        public int Year { get; set; }

        public bool IsActive { get; set; } = true;

        public Guid OwnerId { get; set; }

        public virtual Owner Owner { get; set; }

        [NotMapped]
        public decimal Tolal => Price + Tax;
    }
}