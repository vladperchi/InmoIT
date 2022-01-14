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
using InmoIT.Modules.Inmo.Core.Features.Owners.Commands;
using InmoIT.Modules.Inmo.Core.Features.Owners.Queries;
using InmoIT.Modules.Inmo.Core.Features.Owners.Queries.Export;
using InmoIT.Shared.Core.Attributes;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Dtos.Inmo.Owners;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Inmo.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class OwnersController : BaseController
    {
        /// <response code="200">Return list owners.</response>
        /// <response code="204">List owners not content.</response>
        [HttpGet]
        [HavePermission(PermissionsConstant.Owners.ViewAll)]
        [SwaggerHeader("filter", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Get List Owners.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedOwnerFilter filter)
        {
            var request = Mapper.Map<GetAllOwnersQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <response code="200">Return owner by id.</response>
        /// <response code="404">Owner was not found.</response>
        [HttpGet("{id}")]
        [HavePermission(PermissionsConstant.Owners.View)]
        [SwaggerHeader("filter", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Get Owner By Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, Owner> filter)
        {
            var request = Mapper.Map<GetOwnerByIdQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <response code="201">Return created owner.</response>
        /// <response code="400">Owner already exists.</response>
        /// <response code="500">Owner Internal Server Error.</response>
        [HttpPost]
        [HavePermission(PermissionsConstant.Owners.Register)]
        [SwaggerHeader("command", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Created Owner.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterAsync(RegisterOwnerCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        /// <response code="200">Return updated owner.</response>
        /// <response code="404">Owner was not found.</response>
        /// <response code="500">Owner Internal Server Error.</response>
        [HttpPut]
        [HavePermission(PermissionsConstant.Owners.Update)]
        [SwaggerHeader("command", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Update Owner.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(UpdateOwnerCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        /// <response code="200">Return remove owner.</response>
        /// <response code="404">Owner was not found.</response>
        /// <response code="500">Owner Internal Server Error.</response>
        [HttpDelete("{id}")]
        [HavePermission(PermissionsConstant.Owners.Remove)]
        [SwaggerHeader("id", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Remove Owner.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            var response = await Mediator.Send(new RemoveOwnerCommand(id));
            return Ok(response);
        }

        /// <response code="200">Return export owners to excel.</response>
        [HttpGet("export")]
        [HavePermission(PermissionsConstant.Owners.Export)]
        [SwaggerHeader("searchString", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Export Owners To Excel.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportAsync(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportOwnersQuery(searchString)));
        }
    }
}