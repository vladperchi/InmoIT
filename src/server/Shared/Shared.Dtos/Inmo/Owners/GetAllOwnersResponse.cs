// --------------------------------------------------------------------------------------------------
// <copyright file="GetAllOwnersResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Inmo.Owners
{
    public record GetAllOwnersResponse(Guid Id, string Name, string SurName, string Address, string ImageUrl, string Birthday, string Email, string PhoneNumber, string Gender, string Group);
}