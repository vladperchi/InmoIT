// --------------------------------------------------------------------------------------------------
// <copyright file="LoggerController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using InmoIT.Shared.Infrastructure.Permissions;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Dtos.Identity.Logging;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using InmoIT.Shared.Core.Attributes;

namespace InmoIT.Modules.Identity.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class LoggerController : BaseController
    {
        private readonly ILoggerService _eventLog;

        public LoggerController(ILoggerService eventLog)
        {
            _eventLog = eventLog;
        }

        [HttpGet]
        [HavePermission(PermissionsConstant.Logs.ViewAll)]
        [SwaggerHeader("filter", "Input data not required", "", false)]
        [SwaggerOperation(
            Summary = "Get Log User List.",
            Description = "List all logs user in the database. This can only be done by the registered user",
            OperationId = "GetAllAsync")]
        [SwaggerResponse(200, "Return log user list.")]
        [SwaggerResponse(204, "Log user list not content.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedLogFilter filter)
        {
            var request = Mapper.Map<GetAllLogsRequest>(filter);
            var response = await _eventLog.GetAllAsync(request);
            return Ok(response);
        }

        [HttpPost]
        [HavePermission(PermissionsConstant.Logs.Create)]
        [SwaggerHeader("request", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Created Log Custom Event.",
            Description = "Created a log custom event user with all its values set. This can only be done by the registered user",
            OperationId = "LogCustomEventAsync")]
        [SwaggerResponse(201, "Return created log custom event user.")]
        [SwaggerResponse(500, "Log Custom Event Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> LogCustomEventAsync(LogRequest request)
        {
            var response = await _eventLog.LogCustomEventAsync(request);
            return Ok(response);
        }
    }
}