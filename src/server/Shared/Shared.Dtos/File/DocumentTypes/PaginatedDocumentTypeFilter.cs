// --------------------------------------------------------------------------------------------------
// <copyright file="PaginatedDocumentTypeFilter.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Dtos.Filters;

namespace InmoIT.Shared.Dtos.File.DocumentTypes
{
    public class PaginatedDocumentTypeFilter : PaginatedFilter
    {
        public string SearchString { get; set; }
    }
}