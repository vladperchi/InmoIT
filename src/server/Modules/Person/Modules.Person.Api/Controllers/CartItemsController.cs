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
using InmoIT.Shared.Core.Attributes;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Dtos.Person.CartItems;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Person.Api.Controllers
{
    internal sealed class CartItemsController : BaseController
    {
        /// <response code="200">Return list cart items.</response>
        /// <response code="204">List Cart Items not content.</response>
        [HttpGet]
        [HavePermission(PermissionsConstant.CartItems.ViewAll)]
        [SwaggerHeader("filter", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Get List Cart Items.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetCartItemsAsync([FromQuery] PaginatedCartItemFilter filter)
        {
            var request = Mapper.Map<GetAllCartItemsQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <response code="200">Return cart items by id.</response>
        /// <response code="404">Cart Items was not found.</response>
        [HttpGet("{id}")]
        [HavePermission(PermissionsConstant.CartItems.View)]
        [SwaggerHeader("filter", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Get Cart Items By Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetCartItemByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, CartItem> filter)
        {
            var request = Mapper.Map<GetCartItemByIdQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <response code="201">Return add cart items.</response>
        /// <response code="400">Cart Items already exists.</response>
        /// <response code="500">Cart Items Internal Server Error.</response>
        [HttpPost]
        [HavePermission(PermissionsConstant.CartItems.Add)]
        [SwaggerHeader("command", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Add Cart Items.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddCartItemAsync(AddCartItemCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        /// <response code="200">Return updated cart items.</response>
        /// <response code="404">Cart Items was not found.</response>
        /// <response code="500">Cart Items Internal Server Error.</response>
        [HttpPut]
        [HavePermission(PermissionsConstant.CartItems.Update)]
        [SwaggerHeader("command", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Update Cart Items.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateCartItemAsync(UpdateCartItemCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        /// <response code="200">Return remove cart items.</response>
        /// <response code="404">Cart Items was not found.</response>
        /// <response code="500">Cart Items Internal Server Error.</response>
        [HttpDelete("{id}")]
        [HavePermission(PermissionsConstant.CartItems.Remove)]
        [SwaggerHeader("id", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Remove Cart Items.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RemoveCartItemAsync(Guid id)
        {
            var response = await Mediator.Send(new RemoveCartItemCommand(id));
            return Ok(response);
        }
    }
}