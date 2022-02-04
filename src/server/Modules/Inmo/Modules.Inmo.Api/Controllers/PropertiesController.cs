// --------------------------------------------------------------------------------------------------
// <copyright file="PropertiesController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Features.Properties.Commands;
using InmoIT.Modules.Inmo.Core.Features.Properties.Queries;
using InmoIT.Modules.Inmo.Core.Features.Properties.Queries.Export;
using InmoIT.Shared.Core.Attributes;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Dtos.Inmo.Properties;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Inmo.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class PropertiesController : BaseController
    {
        /// <response code="200">Return list properties.</response>
        /// <response code="204">List properties not content.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpGet]
        [HavePermission(PermissionsConstant.Properties.ViewAll)]
        [SwaggerHeader("filter", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Get List Properties.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedPropertyFilter filter)
        {
            var request = Mapper.Map<GetAllPropertiesQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <response code="200">Return property by id.</response>
        /// <response code="404">Property was not found.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpGet("{id}")]
        [HavePermission(PermissionsConstant.Properties.View)]
        [SwaggerHeader("filter", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Get Property By Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, Property> filter)
        {
            var request = Mapper.Map<GetPropertyByIdQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <response code="201">Return registered property.</response>
        /// <response code="400">Property already exists.</response>
        /// <response code="500">Property Internal Server Error.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpPost]
        [HavePermission(PermissionsConstant.Properties.Register)]
        [SwaggerHeader("command", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Registered Property.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegisterAsync(RegisterPropertyCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        /// <response code="200">Return updated property.</response>
        /// <response code="404">Property was not found.</response>
        /// <response code="500">Property Internal Server Error.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpPut]
        [HavePermission(PermissionsConstant.Properties.Update)]
        [SwaggerHeader("command", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Update Property.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateAsync(UpdatePropertyCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        /// <response code="200">Return remove property.</response>
        /// <response code="404">Property was not found.</response>
        /// <response code="500">Property Internal Server Error.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpDelete("{id}")]
        [HavePermission(PermissionsConstant.Properties.Remove)]
        [SwaggerHeader("id", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Remove Property.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            var response = await Mediator.Send(new RemovePropertyCommand(id));
            return Ok(response);
        }

        /// <response code="200">Return export properties to excel.</response>
        /// <response code="404">Properties was not found.</response>
        /// <response code="500">Export Properties Internal Server Error.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpGet("export")]
        [HavePermission(PermissionsConstant.Properties.Export)]
        [SwaggerHeader("searchString", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Export Properties To Excel.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ExportAsync(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportPropertiesQuery(searchString)));
        }
    }
}