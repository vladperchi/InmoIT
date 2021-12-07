// --------------------------------------------------------------------------------------------------
// <copyright file="GetOwnersResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Flow.Owners
{
    public record GetOwnersResponse(Guid Id, string FirstName, string LastName, string Address, string ImageUrl, string Birthday, string Email, string PhoneNumber);
}