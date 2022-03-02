// --------------------------------------------------------------------------------------------------
// <copyright file="CartsController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Modules.Person.Core.Features.Carts.Commands;
using InmoIT.Modules.Person.Core.Features.Carts.Queries;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Dtos.Person.Carts;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Person.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class CartsController : BaseController
    {
        [HttpGet]
        [HavePermission(PermissionsConstant.Carts.ViewAll)]
        [SwaggerOperation(
            Summary = "Get Cart List.",
            Description = "List all carts in the database. This can only be done by the registered user",
            OperationId = "GetAllAsync")]
        [SwaggerResponse(200, "Return cart list.")]
        [SwaggerResponse(204, "Cart list not content.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedCartFilter filter)
        {
            var request = Mapper.Map<GetAllCartsQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [HavePermission(PermissionsConstant.Carts.View)]
        [SwaggerOperation(
            Summary = "Get Cart By Id.",
            Description = "We get the detail caer by Id. This can only be done by the registered user",
            OperationId = "GetByIdAsync")]
        [SwaggerResponse(200, "Return cart by id.")]
        [SwaggerResponse(404, "Cart was not found.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, Cart> filter)
        {
            var request = Mapper.Map<GetCartByIdQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        [HavePermission(PermissionsConstant.Carts.Create)]
        [SwaggerOperation(
            Summary = "Created Cart.",
            Description = "Created a cart with all its values set. This can only be done by the registered user",
            OperationId = "CreateAsync")]
        [SwaggerResponse(201, "Return created cart.")]
        [SwaggerResponse(404, "Cart was not found.")]
        [SwaggerResponse(500, "Cart Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> CreateAsync(CreateCartCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id:guid}")]
        [HavePermission(PermissionsConstant.Carts.Remove)]
        [SwaggerOperation(
            Summary = "Remove Cart.",
            Description = "We get the removed cart by Id. This can only be done by the registered user",
            OperationId = "UpdateAsync")]
        [SwaggerResponse(200, "Return removed cart.")]
        [SwaggerResponse(404, "Cart was not found.")]
        [SwaggerResponse(500, "Cart Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            return Ok(await Mediator.Send(new RemoveCartCommand(id)));
        }

        [HttpDelete("clear/{id:guid}")]
        [HavePermission(PermissionsConstant.Carts.Remove)]
        [SwaggerOperation(
            Summary = "Remove Cart.",
            Description = "We get the clear cart by Id. This can only be done by the registered user",
            OperationId = "RemoveAsync")]
        [SwaggerResponse(200, "Return removed cart.")]
        [SwaggerResponse(404, "Cart was not found.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> ClearAsync(Guid id)
        {
            return Ok(await Mediator.Send(new ClearCartCommand(id)));
        }
    }
}