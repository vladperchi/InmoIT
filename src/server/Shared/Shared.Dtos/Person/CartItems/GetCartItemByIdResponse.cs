// --------------------------------------------------------------------------------------------------
// <copyright file="GetCartItemByIdResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Person.CartItems
{
    public record GetCartItemByIdResponse(Guid Id, Guid CartId, Guid PropertyId)
    {
        public string PropertyCode { get; set; }

        public string PropertyName { get; set; }

        public string PropertyDetail { get; set; }

        public decimal PropertySalePrice { get; set; }

        public decimal PropertyRentPrice { get; set; }
    }
}