// --------------------------------------------------------------------------------------------------
// <copyright file="GetPropertyTypeByIdQuery.cs" company="InmoIT">
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
using InmoIT.Shared.Dtos.Inmo.PropertyTypes;
using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Queries
{
    public class GetPropertyTypeByIdQuery : IRequest<Result<GetPropertyTypeByIdResponse>>, ICacheable
    {
        public Guid Id { get; protected set; }

        public bool SkipCache { get; protected set; }

        public string CacheKey { get; protected set; }

        public TimeSpan? Expiration { get; protected set; }

        public GetPropertyTypeByIdQuery(Guid propertyTypeId, bool skipCache = false, TimeSpan? expiration = null)
        {
            Id = propertyTypeId;
            SkipCache = skipCache;
            CacheKey = CacheKeys.Common.GetEntityByIdCacheKey<Guid, PropertyType>(propertyTypeId);
            Expiration = expiration;
        }
    }
}