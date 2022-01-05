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
using InmoIT.Shared.Core.Attributes;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Dtos.Customers;
using InmoIT.Shared.Infrastructure.Permissions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Person.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class CustomersController : BaseController
    {
        /// <response code="200">Return list customers.</response>
        /// <response code="204">List customers not content.</response>
        [HttpGet]
        [HavePermission(PermissionsConstant.Customers.ViewAll)]
        [SwaggerHeader("filter", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Get List Customers.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedCustomerFilter filter)
        {
            var request = Mapper.Map<GetAllCustomersQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <response code="200">Return customer by id .</response>
        /// <response code="404">Customer was not found.</response>
        [HttpGet("{id}")]
        [HavePermission(PermissionsConstant.Customers.View)]
        [SwaggerHeader("filter", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Get Customer By Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, Customer> filter)
        {
            var request = Mapper.Map<GetCustomerByIdQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <response code="201">Return created customer.</response>
        /// <response code="400">Customer already exists.</response>
        /// <response code="500">Customer Internal Server Error.</response>
        [HttpPost]
        [HavePermission(PermissionsConstant.Customers.Register)]
        [SwaggerHeader("command", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Created Customer.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterAsync(RegisterCustomerCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        /// <response code="200">Return updated customer.</response>
        /// <response code="404">Customer was not found.</response>
        /// <response code="500">Customer Internal Server Error.</response>
        [HttpPut]
        [HavePermission(PermissionsConstant.Customers.Update)]
        [SwaggerHeader("command", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Update Customer.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(UpdateCustomerCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        /// <response code="200">Return remove customer.</response>
        /// <response code="404">Customer was not found.</response>
        /// <response code="500">Customer Internal Server Error.</response>
        [HttpDelete("{id}")]
        [HavePermission(PermissionsConstant.Customers.Remove)]
        [SwaggerHeader("id", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Remove Customer.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            var response = await Mediator.Send(new RemoveCustomerCommand(id));
            return Ok(response);
        }

        /// <response code="200">Return export customers to excel.</response>
        [HttpGet("export")]
        [HavePermission(PermissionsConstant.Customers.Export)]
        [SwaggerHeader("searchString", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Export Customers To Excel.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportCustomersQuery(searchString)));
        }
    }
}