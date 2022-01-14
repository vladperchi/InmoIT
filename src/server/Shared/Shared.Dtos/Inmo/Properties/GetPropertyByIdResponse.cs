// --------------------------------------------------------------------------------------------------
// <copyright file="GetPropertyByIdResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Inmo.Properties
{
    public record GetPropertyByIdResponse(Guid Id, string Name, string Address, string Description, decimal Price, decimal Tax, string CodeInternal, int Year, bool IsActive, Guid OwnerId);
}