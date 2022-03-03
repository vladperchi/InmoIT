// --------------------------------------------------------------------------------------------------
// <copyright file="GetByIdCacheableFilter.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Contracts;
using InmoIT.Shared.Core.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InmoIT.Shared.Core.Features.Filters
{
    public class GetByIdCacheableFilter<TEntityId, TEntity> : ICacheable
        where TEntity : class, IEntity<TEntityId>
    {
        [FromRoute(Name = "id")]
        public TEntityId Id { get; set; }

        [FromQuery]
        public bool SkipCache { get; set; }

        [FromQuery]
        public string CacheKey => CacheKeys.Common.GetEntityByIdCacheKey<TEntityId, TEntity>(Id);

        [FromQuery]
        [RegularExpression(@"^([0-9]{1}|(?:0[0-9]|1[0-9]|2[0-3])+):([0-5]?[0-9])(?::([0-5]?[0-9])(?:.(\d{1,9}))?)?$")]

        public TimeSpan? Expiration { get; set; }
    }
}