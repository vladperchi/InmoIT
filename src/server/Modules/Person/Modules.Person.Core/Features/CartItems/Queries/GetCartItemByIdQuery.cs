// --------------------------------------------------------------------------------------------------
// <copyright file="GetCartItemByIdQuery.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Queries;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Person.CartItems;
using MediatR;

namespace InmoIT.Modules.Person.Core.Features.CartItems.Queries
{
    public class GetCartItemByIdQuery : IRequest<Result<GetCartItemByIdResponse>>, ICacheable
    {
        public Guid Id { get; protected set; }

        public bool SkipCache { get; protected set; }

        public string CacheKey { get; protected set; }

        public TimeSpan? Expiration { get; protected set; }
    }
}