// --------------------------------------------------------------------------------------------------
// <copyright file="GetAllPropertyTracesResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Inmo.Traces
{
    public record GetAllPropertyTracesResponse(Guid Id, string DateSale, string Name, decimal Value, decimal Tax, Guid PropertyId);
}