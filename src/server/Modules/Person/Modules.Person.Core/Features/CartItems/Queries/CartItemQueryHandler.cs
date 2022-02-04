// --------------------------------------------------------------------------------------------------
// <copyright file="CartItemQueryHandler.cs" company="InmoIT">
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
using InmoIT.Modules.Person.Core.Abstractions;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Modules.Person.Core.Exceptions;
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Core.Integration.Inmo;
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Person.CartItems;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Features.CartItems.Queries
{
    internal class CartItemQueryHandler :
       IRequestHandler<GetAllCartItemsQuery, PaginatedResult<GetAllCartItemsResponse>>,
       IRequestHandler<GetCartItemByIdQuery, Result<GetCartItemByIdResponse>>
    {
        private readonly ICustomerDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CartItemQueryHandler> _localizer;
        private readonly IPropertyService _propertyService;

        public CartItemQueryHandler(
            ICustomerDbContext context,
            IMapper mapper,
            IStringLocalizer<CartItemQueryHandler> localizer,
            IPropertyService propertyService)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _propertyService = propertyService;
        }

        public async Task<PaginatedResult<GetAllCartItemsResponse>> Handle(GetAllCartItemsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<CartItem, GetAllCartItemsResponse>> expression = e => new GetAllCartItemsResponse(e.Id, e.CartId, e.PropertyId);
            var sourse = _context.CartItems
                .AsNoTracking()
                .AsQueryable();
            string ordering = new OrderByConverter().Convert(request.OrderBy);
            sourse = !string.IsNullOrWhiteSpace(ordering)
                ? sourse.OrderBy(ordering)
                : sourse.OrderBy(a => a.Id);
            if (request.CartId != null && !request.CartId.Equals(Guid.Empty))
            {
                sourse = sourse.Where(x => x.CartId.Equals(request.CartId));
            }

            var data = await sourse
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            if (data == null)
            {
                throw new CartItemListEmptyException(_localizer);
            }

            var result = _mapper.Map<PaginatedResult<GetAllCartItemsResponse>>(data);
            foreach (var item in result.Data)
            {
                var detailsProperty = await _propertyService.GetDetailsPropertyAsync(item.PropertyId);
                if (detailsProperty.Succeeded)
                {
                    item.PropertyCode = detailsProperty.Data.CodeInternal;
                    item.PropertyName = detailsProperty.Data.Name;
                    item.PropertyDetail = detailsProperty.Data.Description;
                    item.PropertyPrice = detailsProperty.Data.Price + detailsProperty.Data.Tax;
                }
            }

            return result;
        }

        public async Task<Result<GetCartItemByIdResponse>> Handle(GetCartItemByIdQuery query, CancellationToken cancellationToken)
        {
            var data = await _context.CartItems
                .AsNoTracking()
                .Where(b => b.Id == query.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (data == null)
            {
                throw new CartItemNotFoundException(_localizer);
            }

            var result = _mapper.Map<GetCartItemByIdResponse>(data);
            var detailsProperty = await _propertyService.GetDetailsPropertyAsync(result.PropertyId);
            if (detailsProperty.Succeeded)
            {
                result.PropertyCode = detailsProperty.Data.CodeInternal;
                result.PropertyName = detailsProperty.Data.Name;
                result.PropertyDetail = detailsProperty.Data.Description;
                result.PropertyPrice = detailsProperty.Data.Price + detailsProperty.Data.Tax;
            }

            return await Result<GetCartItemByIdResponse>.SuccessAsync(result);
        }
    }
}