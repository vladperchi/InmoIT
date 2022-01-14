// --------------------------------------------------------------------------------------------------
// <copyright file="GetPropertyByIdQuery.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Queries;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.Properties;
using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.Properties.Queries
{
    public class GetPropertyByIdQuery : IRequest<Result<GetPropertyByIdResponse>>, ICacheable
    {
        public Guid Id { get; protected set; }

        public bool SkipCache { get; protected set; }

        public string CacheKey { get; protected set; }

        public TimeSpan? Expiration { get; protected set; }

        public GetPropertyByIdQuery(Guid propertyId, bool skipCache = false, TimeSpan? expiration = null)
        {
            Id = propertyId;
            SkipCache = skipCache;
            CacheKey = CacheKeys.Common.GetEntityByIdCacheKey<Guid, Property>(propertyId);
            Expiration = expiration;
        }
    }
}