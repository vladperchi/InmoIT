// --------------------------------------------------------------------------------------------------
// <copyright file="PaginatedOwnerFilter.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// The core team: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Dtos.Filters;

namespace InmoIT.Shared.Dtos.Flow.Owner
{
    public class PaginatedOwnerFilter : PaginatedFilter
    {
        public string SearchString { get; set; }
    }
}