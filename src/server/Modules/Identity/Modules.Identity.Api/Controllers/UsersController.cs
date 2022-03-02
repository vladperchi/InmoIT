// --------------------------------------------------------------------------------------------------
// <copyright file="UsersController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Modules.Identity.Infrastructure.Extensions;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Interfaces.Services.Identity;
using InmoIT.Shared.Dtos.Identity.Users;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Identity.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
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

        [HttpGet("user-picture/{id}")]
        [HavePermission(PermissionsConstant.Users.View)]
        [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Client, Duration = 60)]
        [SwaggerOperation(
            Summary = "Get Image User.",
            Description = "We get the picture associated to the user. This can only be done by the registered user",
            OperationId = "GetUserPictureAsync")]
        [SwaggerResponse(200, "Return picture user by id.")]
        [SwaggerResponse(404, "User was not found.")]
        public async Task<IActionResult> GetPictureAsync(string id)
        {
            var response = await _userService.GetPictureAsync(id);
            return Ok(response);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Registed User.",
            Description = "Registed a user with all its values set.",
            OperationId = "RegisterAsync")]
        [SwaggerResponse(201, "Return User Registered.")]
        [SwaggerResponse(400, "User name is in use or Phone number is registered")]
        [SwaggerResponse(500, "Identity Internal Server Error.")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var response = await _userService.RegisterAsync(request, GetOrigin());
            return Ok(response);
        }

        [HttpPut]
        [HavePermission(PermissionsConstant.Users.Edit)]
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

        [HttpPost("user-picture/{userId}")]
        [HavePermission(PermissionsConstant.Users.Edit)]
        [SwaggerOperation(
            Summary = "Update Image User.",
            Description = "We get the image user updated. This can only be done by the registered user",
            OperationId = "UpdateUserPictureAsync")]
        [SwaggerResponse(200, "Return updated image user.")]
        [SwaggerResponse(404, "User was not found.")]
        [SwaggerResponse(500, "Identity Internal Server Error.")]
        public async Task<IActionResult> UpdatePictureAsync(string userId, UpdateUserPictureRequest request)
        {
            var response = await _userService.UpdatePictureAsync(userId, request);
            return Ok(response);
        }

        [HttpGet("user-roles/{id}")]
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
        public async Task<IActionResult> GetRolesByUserAsync(string id)
        {
            var response = await _userService.GetRolesByUserAsync(id);
            return Ok(response);
        }

        [HttpPut("user-roles/{userId}")]
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
        public async Task<IActionResult> UpdateRolesByUserAsync(string userId, UserRolesRequest request)
        {
            var response = await _userService.UpdateRolesByUserAsync(userId, request);
            return Ok(response);
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Confirm Email User.",
            Description = "We get the account email confirmed user.",
            OperationId = "ConfirmEmailAsync")]
        [SwaggerResponse(200, "Return email confirmed user.")]
        [SwaggerResponse(404, "User was not found.")]
        [SwaggerResponse(500, "An error occurred while confirming email.")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            var response = await _userService.ConfirmEmailAsync(userId, code);
            return Ok(response);
        }

        [HttpGet("confirm-phone-number")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Confirm Phone Number User.",
            Description = "We get the phone number confirmed user.",
            OperationId = "ConfirmPhoneNumberAsync")]
        [SwaggerResponse(200, "Return phone number confirmed user.")]
        [SwaggerResponse(404, "User was not found.")]
        [SwaggerResponse(500, "An error occurred while confirming phone number.")]
        public async Task<IActionResult> ConfirmPhoneNumberAsync([FromQuery] string userId, [FromQuery] string code)
        {
            var response = await _userService.ConfirmPhoneNumberAsync(userId, code);
            return Ok(response);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Forgot Password User.",
            Description = "We get the steps to follow to remember password.",
            OperationId = "ForgotPasswordAsync")]
        [SwaggerResponse(200, "Return forgot password user.")]
        [SwaggerResponse(404, "User was not found or Email not confirmed")]
        [SwaggerResponse(500, "Identity errors have occurred.")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var response = await _userService.ForgotPasswordAsync(request, GetOrigin());
            return Ok(response);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Reset Password User.",
            Description = "We get the steps to follow to reset new password.",
            OperationId = "ResetPasswordAsync")]
        [SwaggerResponse(200, "Return reset password user.")]
        [SwaggerResponse(404, "User was not found.")]
        [SwaggerResponse(500, "Identity errors have occurred.")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = await _userService.ResetPasswordAsync(request);
            return Ok(response);
        }

        [HttpPost("change-password")]
        [HavePermission(PermissionsConstant.Users.Edit)]
        [SwaggerOperation(
            Summary = "Change Password User.",
            Description = "We get the steps to follow to change current password. This can only be done by the registered user",
            OperationId = "ChangePasswordAsync")]
        [SwaggerResponse(200, "Return change password user.")]
        [SwaggerResponse(404, "User was not found.")]
        [SwaggerResponse(500, "Identity errors have occurred.")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            string userId = User.GetUserId();
            var response = await _userService.ChangePasswordAsync(userId, request);
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
        public async Task<IActionResult> ExportAsync(string searchString = "")
        {
            var response = await _userService.ExportAsync(searchString);
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

        private string GetOrigin() =>
            $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
    }
}