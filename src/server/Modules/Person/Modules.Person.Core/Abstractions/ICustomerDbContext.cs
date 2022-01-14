// --------------------------------------------------------------------------------------------------
// <copyright file="ICustomerDbContext.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Person.Core.Entities;
using Microsoft.EntityFrameworkCore;
using InmoIT.Shared.Core.Interfaces.Contexts;

namespace InmoIT.Modules.Person.Core.Abstractions
{
    public interface ICustomerDbContext : IDbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
    }
}