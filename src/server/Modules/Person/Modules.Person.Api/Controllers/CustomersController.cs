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
using InmoIT.Shared.Dtos.Person.Customers;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Person.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class CustomersController : BaseController
    {
        [HttpGet]
        [HavePermission(PermissionsConstant.Customers.ViewAll)]
        [SwaggerOperation(
            Summary = "Get Customer List.",
            Description = "List all customers in the database. This can only be done by the registered user",
            OperationId = "GetAllAsync")]
        [SwaggerResponse(200, "Return customer list.")]
        [SwaggerResponse(204, "Customer list not content.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedCustomerFilter filter)
        {
            var request = Mapper.Map<GetAllCustomersQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [HavePermission(PermissionsConstant.Customers.View)]
        [SwaggerOperation(
            Summary = "Get Customer By Id.",
            Description = "We get the detail customer by Id. This can only be done by the registered user",
            OperationId = "GetByIdAsync")]
        [SwaggerResponse(200, "Return customer by id.")]
        [SwaggerResponse(404, "Customer was not found.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, Customer> filter)
        {
            var request = Mapper.Map<GetCustomerByIdQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{customerId:guid}")]
        [HavePermission(PermissionsConstant.Customers.View)]
        [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Client, Duration = 60)]
        [SwaggerOperation(
            Summary = "Get Picture Customer.",
            Description = "We get the picture associated to the customer. This can only be done by the registered user",
            OperationId = "GetPictureAsync")]
        [SwaggerResponse(200, "Return picture customer by id.")]
        [SwaggerResponse(404, "Customer was not found.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetPictureAsync(GetCustomerImageQuery request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        [HavePermission(PermissionsConstant.Customers.Register)]
        [SwaggerOperation(
            Summary = "Registered Customer.",
            Description = "Registed a customer with all its values set. This can only be done by the registered user",
            OperationId = "RegisterAsync")]
        [SwaggerResponse(201, "Return registered customer.")]
        [SwaggerResponse(400, "Customer already exists.")]
        [SwaggerResponse(500, "Customer Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> RegisterAsync(RegisterCustomerCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        [HavePermission(PermissionsConstant.Customers.Update)]
        [SwaggerOperation(
            Summary = "Update Customer.",
            Description = "We get the customer with its modified values.. This can only be done by the registered user",
            OperationId = "UpdateAsync")]
        [SwaggerResponse(200, "Return updated customer.")]
        [SwaggerResponse(404, "Customer was not found.")]
        [SwaggerResponse(500, "Customer Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> UpdateAsync(UpdateCustomerCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        [HavePermission(PermissionsConstant.Customers.Remove)]
        [SwaggerOperation(
            Summary = "Remove Customer.",
            Description = "We get the removed customer by Id. This can only be done by the registered user",
            OperationId = "RemoveAsync")]
        [SwaggerResponse(200, "Return removed customer.")]
        [SwaggerResponse(404, "Customer was not found.")]
        [SwaggerResponse(500, "Customer Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            var response = await Mediator.Send(new RemoveCustomerCommand(id));
            return Ok(response);
        }

        [HttpGet("export")]
        [HavePermission(PermissionsConstant.Customers.Export)]
        [SwaggerOperation(
            Summary = "Export Customers To Excel.",
            Description = "We get an exported excel file of all customers. This can only be done by the registered user",
            OperationId = "ExportAsync")]
        [SwaggerResponse(200, "Return export customers to excel.")]
        [SwaggerResponse(204, "Customer list not content.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> ExportAsync(string searchString = "") => Ok(await Mediator.Send(new ExportCustomersQuery(searchString)));
    }
}