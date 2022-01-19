// --------------------------------------------------------------------------------------------------
// <copyright file="GetAllTracesResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Inmo.Traces
{
    public record GetAllTracesResponse(Guid Id, string Code, string Name, decimal Price, decimal Tax, string Type, DateTime CreatedOn, DateTime UpdatedOn, Guid PropertyId);
}