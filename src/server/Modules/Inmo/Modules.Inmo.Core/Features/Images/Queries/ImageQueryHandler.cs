// --------------------------------------------------------------------------------------------------
// <copyright file="ImageQueryHandler.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InmoIT.Modules.Inmo.Core.Abstractions;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Exceptions;
using InmoIT.Modules.Inmo.Core.Specifications;
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.Images;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.Images.Queries
{
    internal class ImageQueryHandler :
       IRequestHandler<GetAllImagesQuery, PaginatedResult<GetAllPropertyImagesResponse>>,
       IRequestHandler<GetImageByPropertyIdQuery, Result<GetPropertyImageByPropertyIdResponse>>
    {
        private readonly IInmoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ImageQueryHandler> _localizer;

        public ImageQueryHandler(
            IInmoDbContext context,
            IMapper mapper,
            IStringLocalizer<ImageQueryHandler> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedResult<GetAllPropertyImagesResponse>> Handle(GetAllImagesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<PropertyImage, GetAllPropertyImagesResponse>> expression = e => new GetAllPropertyImagesResponse(e.Id, e.ImageUrl, e.Caption, e.Enabled, e.CodeImage, e.PropertyId);
            var sourse = _context.PropertyImages
                .AsNoTracking()
                .AsQueryable();
            string ordering = new OrderByConverter().Convert(request.OrderBy);
            sourse = !string.IsNullOrWhiteSpace(ordering)
                ? sourse.OrderBy(ordering)
                : sourse.OrderBy(x => x.Id);

            if (request.PropertyId != null && !request.PropertyId.Equals(Guid.Empty))
            {
                sourse = sourse.Where(x => x.PropertyId.Equals(request.PropertyId));
            }

            var filterSpec = new ImageFilterSpecification(request.SearchString);
            var data = await sourse
                .AsNoTracking()
                .Specify(filterSpec)
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            if (data == null)
            {
                throw new ImageListEmptyException(_localizer);
            }

            return _mapper.Map<PaginatedResult<GetAllPropertyImagesResponse>>(data);
        }

        public async Task<Result<GetPropertyImageByPropertyIdResponse>> Handle(GetImageByPropertyIdQuery query, CancellationToken cancellationToken)
        {
            var data = await _context.PropertyImages
                .AsNoTracking()
                .Where(x => x.Id == query.Id)
                .Include(x => x.Property)
                .FirstOrDefaultAsync(cancellationToken);

            if (data == null)
            {
                throw new ImageNotFoundException(_localizer);
            }

            var result = _mapper.Map<GetPropertyImageByPropertyIdResponse>(data);
            return await Result<GetPropertyImageByPropertyIdResponse>.SuccessAsync(result);
        }
    }
}