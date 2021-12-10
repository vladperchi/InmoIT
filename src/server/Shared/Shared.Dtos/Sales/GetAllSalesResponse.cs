// --------------------------------------------------------------------------------------------------
// <copyright file="GetAllSalesResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Sales
{
    public record GetAllSalesResponse(Guid Id, string ReferenceNumber, DateTime TimeStamp, string CustomerName, string CustomerPhoneNumber, string CustomerEmail, decimal Tax, decimal Discount, decimal Total, bool IsPaid, string Comment, Guid PropertyId, Guid CustomerId);
}