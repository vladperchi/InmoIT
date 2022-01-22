﻿// --------------------------------------------------------------------------------------------------
// <copyright file="GetImageByIdQuery.cs" company="InmoIT">
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
using InmoIT.Shared.Dtos.Inmo.Images;
using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.Images.Queries
{
    public class GetImageByIdQuery : IRequest<Result<GetPropertyImageByIdResponse>>, ICacheable
    {
        public Guid Id { get; protected set; }

        public bool SkipCache { get; protected set; }

        public string CacheKey { get; protected set; }

        public TimeSpan? Expiration { get; protected set; }

        public GetImageByIdQuery(Guid propertyId, bool skipCache = false, TimeSpan? expiration = null)
        {
            Id = propertyId;
            SkipCache = skipCache;
            CacheKey = CacheKeys.Common.GetEntityByIdCacheKey<Guid, PropertyImage>(propertyId);
            Expiration = expiration;
        }
    }
}