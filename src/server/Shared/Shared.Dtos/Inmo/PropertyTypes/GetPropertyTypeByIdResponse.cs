// --------------------------------------------------------------------------------------------------
// <copyright file="GetPropertyTypeByIdResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Inmo.PropertyTypes
{
    public record GetPropertyTypeByIdResponse(Guid Id, string Name, string Code, string Detail, string ImageUrl, bool IsActive);
}