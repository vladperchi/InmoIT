// --------------------------------------------------------------------------------------------------
// <copyright file="Cart.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Person.Core.Entities
{
    public class Cart : BaseEntity
    {
        public Cart()
        {
            CartItems = new List<CartItem>();
            Timestamp = DateTime.Now;
        }

        public Guid CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public DateTime Timestamp { get; private set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}