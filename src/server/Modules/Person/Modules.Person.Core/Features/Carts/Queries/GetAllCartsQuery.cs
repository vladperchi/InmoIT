// --------------------------------------------------------------------------------------------------
// <copyright file="GetAllCartsQuery.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Person.Carts;

using MediatR;

namespace InmoIT.Modules.Person.Core.Features.Carts.Queries
{
    public class GetAllCartsQuery : IRequest<PaginatedResult<GetAllCartsResponse>>
    {
        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public string[] OrderBy { get; private set; }

        public string SearchString { get; private set; }

        public Guid? CustomerId { get; private set; }
    }
}