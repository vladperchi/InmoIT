// --------------------------------------------------------------------------------------------------
// <copyright file="GetAllCustomersResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Person.Customers
{
    public record GetAllCustomersResponse(Guid Id, string Name, string SurName, string PhoneNumber, string Gender, string Group, string Email, string ImageUrl);
}