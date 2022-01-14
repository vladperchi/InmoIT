// --------------------------------------------------------------------------------------------------
// <copyright file="GetAllCartsResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Person.Carts
{
    public record GetAllCartsResponse(Guid Id, Guid CustomerId, DateTime Timestamp);
}