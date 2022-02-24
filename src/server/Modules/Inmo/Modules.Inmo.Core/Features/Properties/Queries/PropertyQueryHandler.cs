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
            Expression<Func<Property, GetAllPropertiesResponse>> expression = e => new GetAllPropertiesResponse(e.Id, e.Name, e.Address, e.Description, e.SquareMeter, e.NumberRooms, e.NumberBathrooms, e.SalePrice, e.RentPrice, e.SaleTax, e.IncomeTax, e.CodeInternal, e.Year, e.HasParking, e.IsActive, e.OwnerName, e.OwnerId, e.PropertyTypeName, e.PropertyTypeId);

            var source = _context.Properties.AsNoTracking().OrderBy(x => x.Id).AsQueryable();
            if (request.OwnerIds.Length > 0)
            {
                source = source.Where(x => request.OwnerIds.Contains(x.OwnerId));
            }

            if (request.PropertyTypeIds.Length > 0)
            {
                source = source.Where(x => request.PropertyTypeIds.Contains(x.PropertyTypeId));
            }

            string ordering = new OrderByConverter().Convert(request.OrderBy);
            source = !string.IsNullOrWhiteSpace(ordering) ? source.OrderBy(ordering) : source.OrderBy(x => x.Id);
            var filterSpec = new PropertyFilterSpecification(request.SearchString);
            var data = await source.AsNoTracking().Specify(filterSpec).Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            _ = data ?? throw new PropertyListEmptyException(_localizer);
            var result = _mapper.Map<PaginatedResult<GetAllPropertiesResponse>>(data);

            if(!result.Succeeded)
            {
                foreach (var item in result.Data)
                {
                    var image = await _propertyImageService.GetDetailsPropertyImageAsync(item.Id);
                    if (image.Succeeded && image.Data.Enabled)
                    {
                        item.PropertyImageCaption = image.Data.Caption;
                        item.PropertyImageUrl = image.Data.ImageUrl;
                        item.PropertyImageCode = image.Data.CodeImage;
                    }
                }
            }

            return result;
        }

        public async Task<Result<GetPropertyByIdResponse>> Handle(GetPropertyByIdQuery query, CancellationToken cancellationToken)
        {
            var data = await _context.Properties.AsNoTracking().Where(x => x.Id == query.Id).FirstOrDefaultAsync(cancellationToken);
            _ = data ?? throw new PropertyNotFoundException(_localizer, query.Id);
            var result = _mapper.Map<GetPropertyByIdResponse>(data);
            if (result is null)
            {
                var image = await _propertyImageService.GetDetailsPropertyImageAsync(result.Id);
                if (image.Succeeded && image.Data.Enabled)
                {
                    result.PropertyImageCaption = image.Data.Caption;
                    result.PropertyImageUrl = image.Data.ImageUrl;
                    result.PropertyImageCode = image.Data.CodeImage;
                }
            }

            return await Result<GetPropertyByIdResponse>.SuccessAsync(result);
        }
    }
}