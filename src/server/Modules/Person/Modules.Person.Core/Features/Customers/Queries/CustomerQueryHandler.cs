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
using InmoIT.Shared.Dtos.Customers;
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
            Expression<Func<Customer, GetAllCustomersResponse>> expression = e => new GetAllCustomersResponse(e.Id, e.Name, e.SurName, e.PhoneNumber, e.Gender, e.Group, e.Email, e.ImageUrl);

            var queryable = _context.Customers.AsNoTracking().OrderBy(x => x.Id).AsQueryable();
            string ordering = new OrderByConverter().Convert(request.OrderBy);
            queryable = !string.IsNullOrWhiteSpace(ordering) ? queryable.OrderBy(ordering) : queryable.OrderBy(a => a.Id);
            var customerFilterSpec = new CustomerFilterSpecification(request.SearchString);
            var customerList = await queryable
                .Specify(customerFilterSpec)
                .Select(expression)
                .AsNoTracking()
                .ToPaginatedListAsync(request.PageNumber, request.PageSize, _localizer);

            if (customerList == null)
            {
                throw new CustomerListEmptyException(_localizer);
            }

            return _mapper.Map<PaginatedResult<GetAllCustomersResponse>>(customerList);
        }

        public async Task<Result<GetCustomerByIdResponse>> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.Where(c => c.Id == query.Id).FirstOrDefaultAsync(cancellationToken);
            if (customer == null)
            {
                throw new CustomerNotFoundException(_localizer);
            }

            var mappedCustomer = _mapper.Map<GetCustomerByIdResponse>(customer);
            return await Result<GetCustomerByIdResponse>.SuccessAsync(mappedCustomer);
        }

        public async Task<Result<string>> Handle(GetCustomerImageQuery request, CancellationToken cancellationToken)
        {
            string data = await _context.Customers.AsNoTracking()
                .Where(c => c.Id == request.Id)
                .Select(a => a.ImageUrl)
                .FirstOrDefaultAsync(cancellationToken);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}