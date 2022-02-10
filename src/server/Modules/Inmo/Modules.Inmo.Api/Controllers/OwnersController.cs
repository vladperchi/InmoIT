// --------------------------------------------------------------------------------------------------
// <copyright file="OwnersController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Exceptions;
using InmoIT.Modules.Inmo.Core.Features.Owners.Commands;
using InmoIT.Modules.Inmo.Core.Features.Owners.Queries;
using InmoIT.Modules.Inmo.Core.Features.Owners.Queries.Export;
using InmoIT.Shared.Core.Attributes;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Dtos.Inmo.Owners;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Inmo.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class OwnersController : BaseController
    {
        ///// <response code="200">Return owner list.</response>
        ///// <response code="204">Owner list not content.</response>
        ///// <response code="401">Without authorization to access.</response>
        ///// <response code="403">No permission to access.</response>
        [HttpGet]
        [HavePermission(PermissionsConstant.Owners.ViewAll)]
        //[SwaggerHeader("filter", "Input data required", "", true)]
        //[SwaggerOperation(Summary = "Get Owners List.")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedOwnerFilter filter)
        {
            var request = Mapper.Map<GetAllOwnersQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        ///// <response code="200">Return owner by id.</response>
        ///// <response code="404">Owner was not found.</response>
        ///// <response code="401">Without authorization to access.</response>
        ///// <response code="403">No permission to access.</response>
        [HttpGet("{id:guid}")]
        [HavePermission(PermissionsConstant.Owners.View)]
        //[SwaggerHeader("filter", "Input data required", "", true)]
        //[SwaggerOperation(Summary = "Get Owner By Id.")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, Owner> filter)
        {
            var request = Mapper.Map<GetOwnerByIdQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        ///// <response code="200">Return image owner by id.</response>
        ///// <response code="404">Owner was not found.</response>
        ///// <response code="403">No permission to access.</response>
        [HttpGet("{ownerId:guid}")]
        [AllowAnonymous]
        //[ResponseCache(NoStore = false, Location = ResponseCacheLocation.Client, Duration = 60)]
        //[SwaggerHeader("request", "Input data required", "", true)]
        //[SwaggerOperation(Summary = "Get Image Owner.")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPictureAsync(GetOwnerImageQuery request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        ///// <response code="201">Return registered owner.</response>
        ///// <response code="400">Owner already exists.</response>
        ///// <response code="500">Owner Internal Server Error.</response>
        ///// <response code="401">Without authorization to access.</response>
        ///// <response code="403">No permission to access.</response>
        [HttpPost]
        [HavePermission(PermissionsConstant.Owners.Register)]
        //[SwaggerHeader("command", "Input data required", "", true)]
        //[SwaggerOperation(Summary = "Registered Owner.")]
        //[ProducesResponseType(typeof(RegisterOwnerCommand), StatusCodes.Status201Created)]
        //[ProducesResponseType(typeof(OwnerCustomException), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(OwnerCustomException), StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegisterAsync(RegisterOwnerCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        ///// <response code="200">Return updated owner.</response>
        ///// <response code="404">Owner was not found.</response>
        ///// <response code="500">Owner Internal Server Error.</response>
        ///// <response code="401">Without authorization to access.</response>
        ///// <response code="403">No permission to access.</response>
        [HttpPut]
        [HavePermission(PermissionsConstant.Owners.Update)]
        //[SwaggerHeader("command", "Input data required", "", true)]
        //[SwaggerOperation(Summary = "Update Owner.")]
        //[ProducesResponseType(typeof(UpdateOwnerCommand), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(OwnerNotFoundException), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(OwnerCustomException), StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateAsync(UpdateOwnerCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        ///// <response code="200">Return remove owner.</response>
        ///// <response code="404">Owner was not found.</response>
        ///// <response code="500">Owner Internal Server Error.</response>
        ///// <response code="401">Without authorization to access.</response>
        ///// <response code="403">No permission to access.</response>
        [HttpDelete("{id:guid}")]
        [HavePermission(PermissionsConstant.Owners.Remove)]
        //[SwaggerHeader("id", "Input data required", "", true)]
        //[SwaggerOperation(Summary = "Remove Owner.")]
        //[ProducesResponseType(typeof(RemoveOwnerCommand), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(OwnerNotFoundException), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(OwnerCustomException), StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            var response = await Mediator.Send(new RemoveOwnerCommand(id));
            return Ok(response);
        }

        ///// <response code="200">Return export owners to excel.</response>
        ///// <response code="404">Owners was not found.</response>
        ///// <response code="401">Without authorization to access.</response>
        ///// <response code="403">No permission to access.</response>
        [HttpGet("export")]
        [HavePermission(PermissionsConstant.Owners.Export)]
        //[SwaggerHeader("searchString", "Input data required", "", false)]
        //[SwaggerOperation(Summary = "Export Owners To Excel.")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ExportAsync(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportOwnersQuery(searchString)));
        }
    }
}