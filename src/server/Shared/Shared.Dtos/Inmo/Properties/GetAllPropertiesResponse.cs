// --------------------------------------------------------------------------------------------------
// <copyright file="GetAllPropertiesResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Inmo.Properties
{
    public record GetAllPropertiesResponse(Guid Id, string Name, string Address, decimal Price, string CodeInternal, int Year, bool IsPublic, Guid OwnerId);
}