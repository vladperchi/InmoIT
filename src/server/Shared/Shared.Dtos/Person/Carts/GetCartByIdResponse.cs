// --------------------------------------------------------------------------------------------------
// <copyright file="GetCartByIdResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using InmoIT.Shared.Dtos.Person.CartItems;
using InmoIT.Shared.Dtos.Person.Customers;

namespace InmoIT.Shared.Dtos.Person.Carts
{
    public record GetCartByIdResponse(Guid Id, Guid CustomerId, DateTime Timestamp)
    {
        public GetCustomerByIdResponse Customer { get; set; }

        public ICollection<GetCartItemByIdResponse> CartItems { get; set; }
    }
}