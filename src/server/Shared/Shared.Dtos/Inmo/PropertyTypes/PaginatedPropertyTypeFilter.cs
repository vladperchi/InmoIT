// --------------------------------------------------------------------------------------------------
// <copyright file="PaginatedPropertyTypeFilter.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Dtos.Filters;

namespace InmoIT.Shared.Dtos.Inmo.PropertyTypes
{
    public class PaginatedPropertyTypeFilter : PaginatedFilter
    {
        public string SearchString { get; set; }
    }
}