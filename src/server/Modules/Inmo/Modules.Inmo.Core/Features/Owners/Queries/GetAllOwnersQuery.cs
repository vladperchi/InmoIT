// --------------------------------------------------------------------------------------------------
// <copyright file="GetAllOwnersQuery.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.Owners;
using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.Owners.Queries
{
    public class GetAllOwnersQuery : IRequest<PaginatedResult<GetAllOwnersResponse>>
    {
        public string SearchString { get; private set; }

        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public string[] OrderBy { get; private set; }
    }
}