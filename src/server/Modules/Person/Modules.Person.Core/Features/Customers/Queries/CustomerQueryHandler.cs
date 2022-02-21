// --------------------------------------------------------------------------------------------------
// <copyright file="CustomerQueryHandler.cs" company="InmoIT">
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
using InmoIT.Modules.Person.Core.Specifications;
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Person.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Features.Customers.Queries
{
    internal class CustomerQueryHandler :
        IRequestHandler<GetAllCustomersQuery, PaginatedResult<GetAllCustomersResponse>>,
        IRequestHandler<GetCustomerByIdQuery, Result<GetCustomerByIdResponse>>,
        IRequestHandler<GetCustomerImageQuery, Result<string>>
    {
        private readonly ICustomerDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CustomerQueryHandler> _localizer;

        public CustomerQueryHandler(
            ICustomerDbContext context,
            IMapper mapper,
            IStringLocalizer<CustomerQueryHandler> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedResult<GetAllCustomersResponse>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Customer, GetAllCustomersResponse>> expression = e => new GetAllCustomersResponse(e.Id, e.Name, e.SurName, e.PhoneNumber, e.Birthday, e.Gender, e.Group, e.Email, e.ImageUrl);
            var source = _context.Customers.AsNoTracking().OrderBy(x => x.Id).AsQueryable();
            string ordering = new OrderByConverter().Convert(request.OrderBy);
            source = !string.IsNullOrWhiteSpace(ordering)
                ? source.OrderBy(ordering)
                : source.OrderBy(x => x.Id);
            var filterSpec = new CustomerFilterSpecification(request.SearchString);
            var data = await source.AsNoTracking().Specify(filterSpec).Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            if (data is null)
            {
                throw new CustomerListEmptyException(_localizer);
            }

            return _mapper.Map<PaginatedResult<GetAllCustomersResponse>>(data);
        }

        public async Task<Result<GetCustomerByIdResponse>> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
        {
            var data = await _context.Customers.AsNoTracking().Where(c => c.Id == query.Id).FirstOrDefaultAsync(cancellationToken);
            if (data is null)
            {
                throw new CustomerNotFoundException(_localizer, query.Id);
            }

            var result = _mapper.Map<GetCustomerByIdResponse>(data);
            return await Result<GetCustomerByIdResponse>.SuccessAsync(result);
        }

        public async Task<Result<string>> Handle(GetCustomerImageQuery request, CancellationToken cancellationToken)
        {
            string result = await _context.Customers.AsNoTracking().Where(c => c.Id == request.Id).Select(a => a.ImageUrl).FirstOrDefaultAsync(cancellationToken);
            if (result is null)
            {
                throw new CustomerNotFoundException(_localizer, request.Id);
            }

            return await Result<string>.SuccessAsync(data: result);
        }
    }
}