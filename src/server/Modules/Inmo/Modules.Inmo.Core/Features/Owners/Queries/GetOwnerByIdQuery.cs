// --------------------------------------------------------------------------------------------------
// <copyright file="GetOwnerByIdQuery.cs" company="InmoIT">
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
using InmoIT.Shared.Dtos.Inmo.Owners;

using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.Owners.Queries
{
    public class GetOwnerByIdQuery : IRequest<Result<GetOwnerByIdResponse>>, ICacheable
    {
        public Guid Id { get; protected set; }

        public bool SkipCache { get; protected set; }

        public string CacheKey { get; protected set; }

        public TimeSpan? Expiration { get; protected set; }

        public GetOwnerByIdQuery(Guid ownerId, bool skipCache = false, TimeSpan? expiration = null)
        {
            Id = ownerId;
            SkipCache = skipCache;
            CacheKey = CacheKeys.Common.GetEntityByIdCacheKey<Guid, Owner>(ownerId);
            Expiration = expiration;
        }
    }
}