// --------------------------------------------------------------------------------------------------
// <copyright file="CartItemsController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Modules.Person.Core.Features.CartItems.Commands;
using InmoIT.Modules.Person.Core.Features.CartItems.Queries;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Dtos.Person.CartItems;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Person.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class CartItemsController : BaseController
    {
        [HttpGet]
        [HavePermission(PermissionsConstant.CartItems.ViewAll)]
        [SwaggerOperation(
            Summary = "Get Cart Item List.",
            Description = "List all Cart Items in the database. This can only be done by the registered user",
            OperationId = "GetAllAsync")]
        [SwaggerResponse(200, "Return Cart Item list.")]
        [SwaggerResponse(204, "Cart Item list not content.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedCartItemFilter filter)
        {
            var request = Mapper.Map<GetAllCartItemsQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [HavePermission(PermissionsConstant.CartItems.View)]
        [SwaggerOperation(
            Summary = "Get cart items By Id.",
            Description = "We get the detail cart items by Id. This can only be done by the registered user",
            OperationId = "GetByIdAsync")]
        [SwaggerResponse(200, "Return cart items by id.")]
        [SwaggerResponse(404, "Cart items was not found.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, CartItem> filter)
        {
            var request = Mapper.Map<GetCartItemByIdQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        [HavePermission(PermissionsConstant.CartItems.Add)]
        [SwaggerOperation(
            Summary = "Added Cart Items.",
            Description = "Added a cart items with all its values set. This can only be done by the registered user",
            OperationId = "AddAsync")]
        [SwaggerResponse(201, "Return added cart item.")]
        [SwaggerResponse(404, "Cart was not found.")]
        [SwaggerResponse(400, "Cart Item already added to the Cart.")]
        [SwaggerResponse(500, "Cart items Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> AddAsync(AddCartItemCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        [HavePermission(PermissionsConstant.CartItems.Update)]
        [SwaggerOperation(
            Summary = "Update Cart Items.",
            Description = "We get the cart items with its modified values.. This can only be done by the registered user",
            OperationId = "UpdateAsync")]
        [SwaggerResponse(200, "Return updated customer.")]
        [SwaggerResponse(404, "Cart Items was not found.")]
        [SwaggerResponse(400, "Cart Items already added to the Cart.")]
        [SwaggerResponse(500, "Cart Items Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> UpdateAsync(UpdateCartItemCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        [HavePermission(PermissionsConstant.CartItems.Remove)]
        [SwaggerOperation(
            Summary = "Remove Cart Items.",
            Description = "We get the removed cart items by Id. This can only be done by the registered user",
            OperationId = "RemoveAsync")]
        [SwaggerResponse(200, "Return removed customer.")]
        [SwaggerResponse(404, "Cart items was not found.")]
        [SwaggerResponse(500, "Cart items Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            var response = await Mediator.Send(new RemoveCartItemCommand(id));
            return Ok(response);
        }
    }
}