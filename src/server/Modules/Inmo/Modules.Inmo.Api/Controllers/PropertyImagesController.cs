// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyImagesController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Features.Images.Commands;
using InmoIT.Modules.Inmo.Core.Features.Images.Queries;
using InmoIT.Shared.Core.Attributes;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Dtos.Inmo.Images;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Inmo.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class PropertyImagesController : BaseController
    {
        /// <response code="200">Return list property images.</response>
        /// <response code="204">List property images not content.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpGet]
        [HavePermission(PermissionsConstant.Images.ViewAll)]
        [SwaggerHeader("filter", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Get List Property Images.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedPropertyImageFilter filter)
        {
            var request = Mapper.Map<GetAllImagesQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <response code="200">Return property image by id.</response>
        /// <response code="404">Property Image was not found.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpGet("{id}")]
        [HavePermission(PermissionsConstant.Images.View)]
        [SwaggerHeader("filter", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Get Property Image By Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, PropertyImage> filter)
        {
            var request = Mapper.Map<GetImageByPropertyIdQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <response code="201">Return added list property images.</response>
        /// <response code="404">Property was not found.</response>
        /// <response code="400">Property Image already exists.</response>
        /// <response code="500">Property Image Internal Server Error.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpPost]
        [HavePermission(PermissionsConstant.Images.Add)]
        [SwaggerHeader("command", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Added List Property Images.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddAsync(AddImageCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        /// <response code="200">Return updated property image.</response>
        /// <response code="404">Property Image was not found.</response>
        /// <response code="500">Property Image Internal Server Error.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpPut]
        [HavePermission(PermissionsConstant.Images.Edit)]
        [SwaggerHeader("command", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Edit Property Image.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> EditAsync(UpdateImageCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        /// <response code="200">Return remove property image.</response>
        /// <response code="404">Property Image was not found.</response>
        /// <response code="500">Property Image Internal Server Error.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpDelete("{id}")]
        [HavePermission(PermissionsConstant.Images.Remove)]
        [SwaggerHeader("id", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Remove Property Image.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            var response = await Mediator.Send(new RemoveImageCommand(id));
            return Ok(response);
        }
    }
}