// --------------------------------------------------------------------------------------------------
// <copyright file="UsersController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Shared.Core.Attributes;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Dtos.Identity.Users;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        [HttpGet]
        [HavePermission(PermissionsConstant.Users.ViewAll)]
        [SwaggerOperation(
            Summary = "Get User List.",
            Description = "List all users in the database. This can only be done by the registered user",
            OperationId = "GetAllAsync")]
        [SwaggerResponse(200, "Return user list.")]
        [SwaggerResponse(204, "User list not content.")]
        [SwaggerResponse(500, "Identity Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _userService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [HavePermission(PermissionsConstant.Users.View)]
        [SwaggerHeader("id", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Get User By Id.",
            Description = "We get the detail user by Id. This can only be done by the registered user",
            OperationId = "GetByIdAsync")]
        [SwaggerResponse(200, "Return user by id.")]
        [SwaggerResponse(404, "User was not found.")]
        [SwaggerResponse(500, "Identity Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var response = await _userService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpPut]
        [HavePermission(PermissionsConstant.Users.Edit)]
        [SwaggerHeader("request", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Update User.",
            Description = "We get the user with its modified values. This can only be done by the registered user",
            OperationId = "UpdateAsync")]
        [SwaggerResponse(200, "Return updated user.")]
        [SwaggerResponse(404, "User was not found.")]
        [SwaggerResponse(500, "Identity Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> UpdateAsync(UpdateUserRequest request)
        {
            var response = await _userService.UpdateAsync(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [HavePermission(PermissionsConstant.Users.Delete)]
        [SwaggerOperation(
            Summary = "Delete user.",
            Description = "We get the deleted user by Id. This can only be done by the registered user",
            OperationId = "DeleteAsync")]
        [SwaggerResponse(200, "Return deleted user by id.")]
        [SwaggerResponse(404, "User was not found.")]
        [SwaggerResponse(405, "Not allowed to deletion.")]
        [SwaggerResponse(500, "Identity Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var response = await _userService.DeleteAsync(id);
            return Ok(response);
        }

        [HttpGet("roles/{id}")]
        [HavePermission(PermissionsConstant.Users.View)]
        [SwaggerOperation(
            Summary = "Get Roles By User Id.",
            Description = "List all roles by user Id. This can only be done by the registered user",
            OperationId = "GetRolesAsync")]
        [SwaggerResponse(200, "Return roles by user id.")]
        [SwaggerResponse(404, "User was not found.")]
        [SwaggerResponse(500, "Identity Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetRolesAsync(string id)
        {
            var response = await _userService.GetRolesAsync(id);
            return Ok(response);
        }

        [HttpPut("roles/{id}")]
        [HavePermission(PermissionsConstant.Users.Edit)]
        [SwaggerOperation(
            Summary = "Update User Roles.",
            Description = "We get the user roles modified. This can only be done by the registered user",
            OperationId = "UpdateUserRolesAsync")]
        [SwaggerResponse(200, "Return updated user roles by user id.")]
        [SwaggerResponse(404, "User was not found.")]
        [SwaggerResponse(405, "Not allowed to change.")]
        [SwaggerResponse(500, "Identity Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> UpdateUserRolesAsync(string id, UserRolesRequest request)
        {
            var response = await _userService.UpdateUserRolesAsync(id, request);
            return Ok(response);
        }

        [HttpGet("export")]
        [HavePermission(PermissionsConstant.Users.Export)]
        [SwaggerOperation(
            Summary = "Export Users To Excel.",
            Description = "We get an exported excel file of all users. This can only be done by the registered user",
            OperationId = "ExportAsync")]
        [SwaggerResponse(200, "Return export users to excel.")]
        [SwaggerResponse(204, "User list not content.")]
        [SwaggerResponse(500, "Identity Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> ExportAsync(string searchString = "") => Ok(await _userService.ExportAsync(searchString));
    }
}