// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyQueryHandler.cs" company="InmoIT">
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
using InmoIT.Shared.Core.Integration.Inmo;
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.Properties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.Properties.Queries
{
    internal class PropertyQueryHandler :
        IRequestHandler<GetAllPropertiesQuery, PaginatedResult<GetAllPropertiesResponse>>,
        IRequestHandler<GetPropertyByIdQuery, Result<GetPropertyByIdResponse>>
    {
        private readonly IInmoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<PropertyQueryHandler> _localizer;
        private readonly IPropertyImageService _propertyImageService;

        public PropertyQueryHandler(
            IInmoDbContext context,
            IMapper mapper,
            IStringLocalizer<PropertyQueryHandler> localizer,
            IPropertyImageService propertyImageService)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _propertyImageService = propertyImageService;
        }

        public async Task<PaginatedResult<GetAllPropertiesResponse>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Property, GetAllPropertiesResponse>> expression = e => new GetAllPropertiesResponse(e.Id, e.Name, e.Address, e.Description, e.Price, e.Tax, e.CodeInternal, e.Year, e.IsActive, e.OwnerId);
            var source = _context.Properties
                .Where(x => x.IsActive)
                .AsNoTracking()
                .AsQueryable();
            if (request.OwnerIds.Length > 0)
            {
                source = source.Where(x => request.OwnerIds.Contains(x.OwnerId));
            }

            string ordering = new OrderByConverter().Convert(request.OrderBy);
            source = !string.IsNullOrWhiteSpace(ordering)
                ? source.OrderBy(ordering)
                : source.OrderBy(x => x.Id);

            var filterSpec = new PropertyFilterSpecification(request.SearchString);
            var data = await source
                .AsNoTracking()
                .Specify(filterSpec)
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize, _localizer);
            if (data == null)
            {
                throw new PropertyListEmptyException(_localizer);
            }

            var result = _mapper.Map<PaginatedResult<GetAllPropertiesResponse>>(data);
            foreach (var item in result.Data)
            {
                var propertyImage = await _propertyImageService.GetDetailsPropertyImageAsync(item.Id);
                if (propertyImage.Succeeded)
                {
                    if (propertyImage.Data.Enabled)
                    {
                        item.PropertyImageCaption = propertyImage.Data.Caption;
                        item.PropertyImageUrl = propertyImage.Data.ImageUrl;
                    }
                }
            }

            return result;
        }

        public async Task<Result<GetPropertyByIdResponse>> Handle(GetPropertyByIdQuery query, CancellationToken cancellationToken)
        {
            var data = await _context.Properties
                .AsNoTracking()
                .Where(x => x.Id == query.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (data == null)
            {
                throw new PropertyNotFoundException(_localizer);
            }

            var result = _mapper.Map<GetPropertyByIdResponse>(data);
            return await Result<GetPropertyByIdResponse>.SuccessAsync(result);
        }
    }
}