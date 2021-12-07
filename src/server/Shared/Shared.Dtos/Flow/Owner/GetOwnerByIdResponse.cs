﻿// --------------------------------------------------------------------------------------------------
// <copyright file="GetOwnerByIdResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Flow.Owner
{
    public record GetOwnerByIdResponse(Guid Id, string Name, string Address, string Email, string PhoneNumber, string ImageUrl, string Birthday);
}