// --------------------------------------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Person.Core.Abstractions;
using InmoIT.Modules.Person.Core.Features.Customers.Commands;
using InmoIT.Modules.Person.Core.Features.Customers.Queries;
using InmoIT.Shared.Core.Integration.Person;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Person.Customers;
using InmoIT.Shared.Infrastructure.Common;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Person.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerDbContext _context;
        private readonly IMediator _mediator;

        public CustomerService(
            ICustomerDbContext context,
            IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Result<GetCustomerByIdResponse>> GetDetailsCustomerAsync(Guid customerId)
        {
            return await _mediator.Send(new GetCustomerByIdQuery(customerId, true));
        }

        public async Task<Result<Guid>> RemoveCustomerAsync(Guid customerId)
        {
            return await _mediator.Send(new RemoveCustomerCommand(customerId));
        }

        public async Task<string> GenerateFileName(int length)
        {
            return await Utilities.GenerateCode("C", length);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Customers.CountAsync();
        }
    }
}