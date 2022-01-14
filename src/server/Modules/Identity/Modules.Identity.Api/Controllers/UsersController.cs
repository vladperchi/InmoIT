// --------------------------------------------------------------------------------------------------
// <copyright file="UsersController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Shared.Infrastructure.Permissions;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Dtos.Identity.Users;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;
using InmoIT.Shared.Core.Attributes;

namespace InmoIT.Modules.Identity.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(
            IUserService userService)
{
            _userService = userService;
        }

        /// <response code="200">Return list users.</response>
        /// <response code="204">User not content.</response>
        /// <response code="500">Identity Internal Server Error.</response>
        [HttpGet]
        [HavePermission(PermissionsConstant.Users.ViewAll)]
        [SwaggerOperation(Summary = "Get List Users.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _userService.GetAllAsync();
            return Ok(response);
        }

        /// <response code="200">Return user.</response>
        /// <response code="404">User was not found.</response>
        /// <response code="500">Identity Internal Server Error.</response>
        [HttpGet("{id}")]
        [HavePermission(PermissionsConstant.Users.View)]
        [SwaggerHeader("id", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "User By Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var response = await _userService.GetByIdAsync(id);
            return Ok(response);
        }

        /// <response code="200">Return updated user.</response>
        /// <response code="404">User was not found.</response>
        /// <response code="500">Identity Internal Server Error.</response>
        [HttpPut]
        [HavePermission(PermissionsConstant.Users.Edit)]
        [SwaggerHeader("request", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Update User.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateAsync(UpdateUserRequest request)
        {
            var response = await _userService.UpdateAsync(request);
            return Ok(response);
        }

        /// <response code="200">Return roles user.</response>
        /// <response code="404">User was not found.</response>
        [HttpGet("roles/{id}")]
        [HavePermission(PermissionsConstant.Users.View)]
        [SwaggerHeader("id", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Get Roles User.", Description = "")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetRolesAsync(string id)
        {
            var response = await _userService.GetRolesAsync(id);
            return Ok(response);
        }

        /// <response code="200">Return user roles.</response>
        /// <response code="404">User was not found.</response>
        [HttpPut("roles/{id}")]
        [HavePermission(PermissionsConstant.Users.Edit)]
        [SwaggerHeader("id, request", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Get User Roles.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateUserRolesAsync(string id, UserRolesRequest request)
        {
            var response = await _userService.UpdateUserRolesAsync(id, request);
            return Ok(response);
        }

        /// <response code="200">Return export users to excel.</response>
        [HttpGet("export")]
        [HavePermission(PermissionsConstant.Users.Export)]
        [SwaggerHeader("searchString", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Export Users To Excel.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ExportUserAsync(string searchString = "")
        {
            return Ok(await _userService.ExportUserAsync(searchString));
        }
    }
}