﻿// --------------------------------------------------------------------------------------------------
// <copyright file="PaginatedOwnerFilter.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Dtos.Filters;

namespace InmoIT.Shared.Dtos.Inmo.Owners
{
    public class PaginatedOwnerFilter : PaginatedFilter
    {
        public string SearchString { get; set; }
    }
}