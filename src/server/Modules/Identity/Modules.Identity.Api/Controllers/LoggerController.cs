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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using InmoIT.Shared.Core.Attributes;
using Microsoft.AspNetCore.Http;

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

        /// <response code="200">Return all log list user.</response>
        /// <response code="204">Log not content.</response>
        [HttpGet]
        [HavePermission(PermissionsConstant.Logs.ViewAll)]
        [SwaggerHeader("filter", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Get All Logs User.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedLogFilter filter)
        {
            var request = Mapper.Map<GetAllLogsRequest>(filter);
            var response = await _eventLog.GetAllAsync(request);
            return Ok(response);
        }

        /// <response code="201">Return created log user.</response>
        /// <response code="400">Log errors have occurred.</response>
        [HttpPost]
        [Authorize]
        [SwaggerHeader("request", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Created Log User.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LogCustomEventAsync(LogRequest request)
        {
            var response = await _eventLog.LogCustomEventAsync(request);
            return Ok(response);
        }
    }
}