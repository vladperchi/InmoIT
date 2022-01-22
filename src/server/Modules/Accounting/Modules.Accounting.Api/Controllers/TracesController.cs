// --------------------------------------------------------------------------------------------------
// <copyright file="TracesController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using InmoIT.Modules.Accounting.Core.Features.Traces.Queries;
using InmoIT.Modules.Accounting.Core.Features.Traces.Queries.Export;
using InmoIT.Shared.Core.Attributes;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Dtos.Inmo.Traces;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Accounting.Api.Controllers
{
    [ApiVersion("1")]
    internal class TracesController : BaseController
    {
        /// <response code="200">Return property trace list.</response>
        /// <response code="204">Property trace list not content.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpGet]
        [HavePermission(PermissionsConstant.Traces.ViewAll)]
        [SwaggerHeader("filter", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Get Property Trace List.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedTraceFilter filter)
        {
            var request = Mapper.Map<GetAllTracesQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <response code="200">Return export property traces to excel.</response>
        /// <response code="404">Trace was not found.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpGet("export")]
        [HavePermission(PermissionsConstant.Traces.Export)]
        [SwaggerHeader("searchString", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Export Property Traces To Excel.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ExportAsync(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportTracesQuery(searchString)));
        }
    }
}