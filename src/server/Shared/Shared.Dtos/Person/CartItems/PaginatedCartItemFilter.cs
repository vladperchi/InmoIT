﻿// --------------------------------------------------------------------------------------------------
// <copyright file="PaginatedCartItemFilter.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Dtos.Filters;

namespace InmoIT.Shared.Dtos.Person.CartItems
{
    public class PaginatedCartItemFilter : PaginatedFilter
    {
        public string SearchString { get; set; }

        public Guid? CartId { get; set; }

        public Guid? PropertyId { get; set; }
    }
}