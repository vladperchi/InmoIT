// --------------------------------------------------------------------------------------------------
// <copyright file="PaginatedImageFilter.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

using InmoIT.Shared.Dtos.Filters;

namespace InmoIT.Shared.Dtos.Flow.Image
{
    public class PaginatedImageFilter : PaginatedFilter
    {
        public string SearchString { get; set; }

        public Guid[] PropertyIds { get; set; }
    }
}