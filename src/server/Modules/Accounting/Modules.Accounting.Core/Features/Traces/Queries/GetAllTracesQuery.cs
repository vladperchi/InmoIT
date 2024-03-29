﻿// --------------------------------------------------------------------------------------------------
// <copyright file="GetAllTracesQuery.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Accounting.Traces;
using MediatR;

namespace InmoIT.Modules.Accounting.Core.Features.Traces.Queries
{
    public class GetAllTracesQuery : IRequest<PaginatedResult<GetAllTracesResponse>>
    {
        public string SearchString { get; private set; }

        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public string[] OrderBy { get; private set; }

        public Guid? PropertyId { get; private set; }
    }
}