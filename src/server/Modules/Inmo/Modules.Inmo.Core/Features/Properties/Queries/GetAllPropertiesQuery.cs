// --------------------------------------------------------------------------------------------------
// <copyright file="GetAllPropertiesQuery.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.Properties;
using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.Properties.Queries
{
    public class GetAllPropertiesQuery : IRequest<PaginatedResult<GetAllPropertiesResponse>>
    {
        public string SearchString { get; private set; }

        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public string[] OrderBy { get; private set; }

        public Guid[] OwnerIds { get; private set; }

        public Guid[] PropertyTypeIds { get; private set; }
    }
}