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
using InmoIT.Shared.Dtos.Accounting.Traces;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Accounting.Api.Controllers
{
    [ApiVersion("1")]
    internal class TracesController : BaseController
    {
        [HttpGet]
        [HavePermission(PermissionsConstant.Traces.ViewAll)]
        [SwaggerHeader("filter", "Input data not required", "", false)]
        [SwaggerOperation(
            Summary = "Get Property Trace List.",
            Description = "List all customers in the database. This can only be done by the registered user",
            OperationId = "GetAllAsync")]
        [SwaggerResponse(200, "Return property trace list.")]
        [SwaggerResponse(204, "Property trace list not content.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedTraceFilter filter)
        {
            var request = Mapper.Map<GetAllTracesQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("export")]
        [HavePermission(PermissionsConstant.Traces.Export)]
        [SwaggerHeader("searchString", "Input data required", "", false)]
        [SwaggerOperation(
            Summary = "Export Property Traces To Excel.",
            Description = "We get an exported excel file of all property traces. This can only be done by the registered user",
            OperationId = "ExportAsync")]
        [SwaggerResponse(200, "Return export property traces to excel.")]
        [SwaggerResponse(204, "Trace list not content.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> ExportAsync(string searchString = "") => Ok(await Mediator.Send(new ExportTracesQuery(searchString)));
    }
}