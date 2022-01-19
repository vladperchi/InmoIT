// --------------------------------------------------------------------------------------------------
// <copyright file="GetAllImagesQuery.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.Images;
using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.Images.Queries
{
    public class GetAllImagesQuery : IRequest<PaginatedResult<GetAllPropertyImagesResponse>>
    {
        public string SearchString { get; private set; }

        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public string[] OrderBy { get; private set; }

        public Guid? PropertyId { get; private set; }
    }
}