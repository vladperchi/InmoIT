// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyTrace.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Inmo.Core.Entities
{
    public class PropertyTrace : BaseEntity
    {
        public string Name { get; set; }

        public string DateSale { get; set; }

        public decimal Value { get; set; }

        public decimal Tax { get; set; }

        public Guid PropertyId { get; set; }

        public virtual Property Property { get; set; }
    }
}