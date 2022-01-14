// --------------------------------------------------------------------------------------------------
// <copyright file="CartItem.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Person.Core.Entities
{
    public class CartItem : BaseEntity
    {
        public Guid CartId { get; set; }

        public virtual Cart Cart { get; set; }

        public Guid PropertyId { get; set; }
    }
}