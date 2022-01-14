// --------------------------------------------------------------------------------------------------
// <copyright file="CartQueryHandler.cs" company="InmoIT">
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
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Person.Carts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Features.Carts.Queries
{
    internal class CartQueryHandler :
       IRequestHandler<GetAllCartsQuery, PaginatedResult<GetAllCartsResponse>>,
       IRequestHandler<GetCartByIdQuery, Result<GetCartByIdResponse>>
    {
        private readonly ICustomerDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CartQueryHandler> _localizer;

        public CartQueryHandler(
            ICustomerDbContext context,
            IMapper mapper,
            IStringLocalizer<CartQueryHandler> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedResult<GetAllCartsResponse>> Handle(GetAllCartsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Cart, GetAllCartsResponse>> expression = e => new GetAllCartsResponse(e.Id, e.CustomerId, e.Timestamp);
            var sourse = _context.Carts
                .AsNoTracking()
                .AsQueryable();
            string ordering = new OrderByConverter().Convert(request.OrderBy);
            sourse = !string.IsNullOrWhiteSpace(ordering)
                ? sourse.OrderBy(ordering)
                : sourse.OrderBy(a => a.Id);
            if (request.CustomerId != null && !request.CustomerId.Equals(Guid.Empty))
            {
                sourse = sourse.Where(x => x.CustomerId.Equals(request.CustomerId));
            }

            var data = await sourse
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize, _localizer);
            if (data == null)
            {
                throw new CartListEmptyException(_localizer);
            }

            return _mapper.Map<PaginatedResult<GetAllCartsResponse>>(data);
        }

        public async Task<Result<GetCartByIdResponse>> Handle(GetCartByIdQuery query, CancellationToken cancellationToken)
        {
            var data = await _context.Carts.AsNoTracking()
                .Where(c => c.Id == query.Id)
                .Include(a => a.CartItems)
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(cancellationToken);

            if (data == null)
            {
                throw new CartNotFoundException(_localizer);
            }

            var result = _mapper.Map<GetCartByIdResponse>(data);
            return await Result<GetCartByIdResponse>.SuccessAsync(result);
        }
    }
}