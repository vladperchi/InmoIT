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
using InmoIT.Shared.Core.Attributes;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Dtos.Person.Carts;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Person.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class CartsController : BaseController
    {
        /// <response code="200">Return list carts.</response>
        /// <response code="204">List carts not content.</response>
        [HttpGet]
        [HavePermission(PermissionsConstant.Carts.ViewAll)]
        [SwaggerHeader("filter", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Get List Carts.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedCartFilter filter)
        {
            var request = Mapper.Map<GetAllCartsQuery>(filter);
            var carts = await Mediator.Send(request);
            return Ok(carts);
        }

        /// <response code="200">Return cart by id.</response>
        /// <response code="404">Cart was not found.</response>
        [HttpGet("{id}")]
        [HavePermission(PermissionsConstant.Carts.View)]
        [SwaggerHeader("filter", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Get Cart By Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, Cart> filter)
        {
            var request = Mapper.Map<GetCartByIdQuery>(filter);
            var cart = await Mediator.Send(request);
            return Ok(cart);
        }

        /// <response code="201">Return created cart.</response>
        /// <response code="400">Cart already exists.</response>
        /// <response code="500">Cart Internal Server Error.</response>
        [HttpPost]
        [HavePermission(PermissionsConstant.Carts.Create)]
        [SwaggerHeader("command", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Created Cart.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateAsync(CreateCartCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <response code="200">Return remove cart.</response>
        /// <response code="404">Cart was not found.</response>
        /// <response code="500">Cart Internal Server Error.</response>
        [HttpDelete("{id}")]
        [HavePermission(PermissionsConstant.Carts.Remove)]
        [SwaggerHeader("id", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Remove Cart.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            return Ok(await Mediator.Send(new RemoveCartCommand(id)));
        }

        /// <response code="200">Return clear cart.</response>
        /// <response code="404">Cart was not found.</response>
        [HttpDelete("clear/{id}")]
        [HavePermission(PermissionsConstant.Carts.Remove)]
        [SwaggerHeader("id", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Clear Cart.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ClearAsync(Guid id)
        {
            return Ok(await Mediator.Send(new ClearCartCommand(id)));
        }
    }
}