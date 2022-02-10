// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyTypesController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Commands;
using InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Queries;
using InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Queries.Export;
using InmoIT.Shared.Core.Attributes;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Dtos.Inmo.PropertyTypes;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Inmo.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class PropertyTypesController : BaseController
    {
        ///// <response code="200">Return property type list.</response>
        ///// <response code="204">Property Type list not content.</response>
        ///// <response code="401">Without authorization to access.</response>
        ///// <response code="403">No permission to access.</response>
        [HttpGet]
        [HavePermission(PermissionsConstant.PropertyTypes.ViewAll)]
        //[SwaggerHeader("filter", "Input data required", "", true)]
        //[SwaggerOperation(Summary = "Get Property Type List.")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedPropertyTypeFilter filter)
        {
            var request = Mapper.Map<GetAllPropertyTypesQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        ///// <response code="200">Return property type by id.</response>
        ///// <response code="404">Property Type was not found.</response>
        ///// <response code="401">Without authorization to access.</response>
        ///// <response code="403">No permission to access.</response>
        [HttpGet("{id:guid}")]
        [HavePermission(PermissionsConstant.PropertyTypes.View)]
        //[SwaggerHeader("filter", "Input data required", "", true)]
        //[SwaggerOperation(Summary = "Get Property Type By Id.")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, PropertyType> filter)
        {
            var request = Mapper.Map<GetPropertyTypeByIdQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        ///// <response code="200">Return created property type.</response>
        ///// <response code="400">Property Type already exists.</response>
        ///// <response code="500">Internal Server Error.</response>
        ///// <response code="401">Without authorization to access.</response>
        ///// <response code="403">No permission to access.</response>
        [HttpPost]
        [HavePermission(PermissionsConstant.PropertyTypes.Create)]
        //[SwaggerHeader("command", "Input data required", "", true)]
        //[SwaggerOperation(Summary = "Created Property Type.")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateAsync(CreatePropertyTypeCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        ///// <response code="200">Return updated property type.</response>
        ///// <response code="404">Property Type  was not found.</response>
        ///// <response code="500">Internal Server Error.</response>
        ///// <response code="401">Without authorization to access.</response>
        ///// <response code="403">No permission to access.</response>
        [HttpPut]
        [HavePermission(PermissionsConstant.PropertyTypes.Update)]
        //[SwaggerHeader("command", "Input data required", "", true)]
        //[SwaggerOperation(Summary = "Update Property Type.")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateAsync(UpdatePropertyTypeCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        ///// <response code="200">Return remove property type.</response>
        ///// <response code="404">Property Type  was not found.</response>
        ///// <response code="500">Internal Server Error.</response>
        ///// <response code="401">Without authorization to access.</response>
        ///// <response code="403">No permission to access.</response>
        [HttpDelete("{id:guid}")]
        [HavePermission(PermissionsConstant.PropertyTypes.Remove)]
        //[SwaggerHeader("id", "Input data required", "", true)]
        //[SwaggerOperation(Summary = "Remove Property Type.")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            var response = await Mediator.Send(new RemovePropertyTypeCommand(id));
            return Ok(response);
        }

        ///// <response code="200">Return picture property type.</response>
        ///// <response code="404">Picture property type was not found.</response>
        ///// <response code="401">Without authorization to access.</response>
        ///// <response code="403">No permission to access.</response>
        [HttpGet("{propertyTypeId:guid}")]
        [AllowAnonymous]
        //[ResponseCache(NoStore = false, Location = ResponseCacheLocation.Client, Duration = 60)]
        //[SwaggerHeader("request", "Input data required", "", true)]
        //[SwaggerOperation(Summary = "Get Image Property Type.")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPictureAsync(GetPropertyTypeImageQuery request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        ///// <response code="200">Return export property types to excel.</response>
        ///// <response code="404">Property Type  was not found.</response>
        ///// <response code="401">Without authorization to access.</response>
        ///// <response code="403">No permission to access.</response>
        [HttpGet("export")]
        [HavePermission(PermissionsConstant.PropertyTypes.Export)]
        //[SwaggerHeader("searchString", "Input data required", "", false)]
        //[SwaggerOperation(Summary = "Export Property Types To Excel.")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ExportAsync(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportPropertyTypesQuery(searchString)));
        }
    }
}