// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyTypeQueryHandler.cs" company="InmoIT">
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
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.PropertyTypes;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using InmoIT.Modules.Inmo.Core.Specifications;

namespace InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Queries
{
    internal class PropertyTypeQueryHandler :
        IRequestHandler<GetAllPropertyTypesQuery, PaginatedResult<GetAllPropertyTypesResponse>>,
        IRequestHandler<GetPropertyTypeByIdQuery, Result<GetPropertyTypeByIdResponse>>,
        IRequestHandler<GetPropertyTypeImageQuery, Result<string>>
    {
        private readonly IInmoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<PropertyTypeQueryHandler> _localizer;

        public PropertyTypeQueryHandler(
            IInmoDbContext context,
            IMapper mapper,
            IStringLocalizer<PropertyTypeQueryHandler> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedResult<GetAllPropertyTypesResponse>> Handle(GetAllPropertyTypesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<PropertyType, GetAllPropertyTypesResponse>> expression = x => new GetAllPropertyTypesResponse(x.Id, x.Name, x.CodeInternal, x.Description, x.ImageUrl, x.IsActive);
            var sourse = _context.PropertyTypes
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .AsQueryable();
            string ordering = new OrderByConverter().Convert(request.OrderBy);
            sourse = !string.IsNullOrWhiteSpace(ordering)
                ? sourse.OrderBy(ordering)
                : sourse.OrderBy(x => x.Id);
            var filterSpec = new PropertyTypeFilterSpecification(request.SearchString);
            var data = await sourse
                .AsNoTracking()
                .Specify(filterSpec)
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            if (data == null)
            {
                throw new PropertyTypeListEmptyException(_localizer);
            }

            return _mapper.Map<PaginatedResult<GetAllPropertyTypesResponse>>(data);
        }

        public async Task<Result<GetPropertyTypeByIdResponse>> Handle(GetPropertyTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var data = await _context.PropertyTypes
                .AsNoTracking()
                .Where(x => x.Id == query.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (data == null)
            {
                throw new PropertyTypeNotFoundException(_localizer);
            }

            var result = _mapper.Map<GetPropertyTypeByIdResponse>(data);
            return await Result<GetPropertyTypeByIdResponse>.SuccessAsync(result);
        }

        public async Task<Result<string>> Handle(GetPropertyTypeImageQuery request, CancellationToken cancellationToken)
        {
            string result = await _context.PropertyTypes
                .AsNoTracking()
                .Where(c => c.Id == request.Id)
                .Select(a => a.ImageUrl)
                .FirstOrDefaultAsync(cancellationToken);
            if (result == null)
            {
                throw new PropertyTypeNotFoundException(_localizer);
            }

            return await Result<string>.SuccessAsync(result);
        }
    }
}