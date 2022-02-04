// --------------------------------------------------------------------------------------------------
// <copyright file="GetPropertyTypeImageQuery.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Wrapper;
using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Queries
{
    public class GetPropertyTypeImageQuery : IRequest<Result<string>>
    {
        public Guid Id { get; }

        public GetPropertyTypeImageQuery(Guid propertyTypeId)
        {
            Id = propertyTypeId;
        }
    }
}