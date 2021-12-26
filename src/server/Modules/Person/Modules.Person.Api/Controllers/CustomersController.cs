// --------------------------------------------------------------------------------------------------
// <copyright file="CustomersController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Modules.Person.Core.Features.Customers.Commands;
using InmoIT.Modules.Person.Core.Features.Customers.Queries;
using InmoIT.Modules.Person.Core.Features.Customers.Queries.Export;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Dtos.Customers;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace InmoIT.Modules.Person.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class CustomersController : BaseController
    {
        [HttpGet]
        [HavePermission(PermissionsConstant.Customers.ViewAll)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedCustomerFilter filter)
        {
            var request = Mapper.Map<GetAllCustomersQuery>(filter);
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("{id}")]
        [HavePermission(PermissionsConstant.Customers.View)]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, Customer> filter)
        {
            var request = Mapper.Map<GetCustomerByIdQuery>(filter);
            return Ok(await Mediator.Send(request));
        }

        [HttpPost]
        [HavePermission(PermissionsConstant.Customers.Register)]
        public async Task<IActionResult> RegisterAsync(RegisterCustomerCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut]
        [HavePermission(PermissionsConstant.Customers.Update)]
        public async Task<IActionResult> UpdateAsync(UpdateCustomerCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        [HavePermission(PermissionsConstant.Customers.Remove)]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            return Ok(await Mediator.Send(new RemoveCustomerCommand(id)));
        }

        [HttpGet("export")]
        [HavePermission(PermissionsConstant.Customers.Export)]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportCustomersQuery(searchString)));
        }
    }
}