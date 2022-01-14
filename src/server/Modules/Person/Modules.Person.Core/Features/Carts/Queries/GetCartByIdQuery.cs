// --------------------------------------------------------------------------------------------------
// <copyright file="GetCartByIdQuery.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Queries;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Person.Carts;
using MediatR;

namespace InmoIT.Modules.Person.Core.Features.Carts.Queries
{
    public class GetCartByIdQuery : IRequest<Result<GetCartByIdResponse>>, ICacheable
    {
        public GetCartByIdQuery()
        {
        }

        public Guid Id { get; protected set; }

        public bool SkipCache { get; protected set; }

        public string CacheKey { get; protected set; }

        public TimeSpan? Expiration { get; protected set; }

        public GetCartByIdQuery(Guid cartId, bool skipCache = false, TimeSpan? expiration = null)
        {
            Id = cartId;
            SkipCache = skipCache;
            CacheKey = CacheKeys.Common.GetEntityByIdCacheKey<Guid, Cart>(cartId);
            Expiration = expiration;
        }
    }
}