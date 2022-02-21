// --------------------------------------------------------------------------------------------------
// <copyright file="IdentityController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Shared.Core.Interfaces.Services.Identity;
using InmoIT.Shared.Dtos.Identity.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InmoIT.Shared.Core.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Identity.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class IdentityController : BaseController
    {
        private readonly ICurrentUser _currentUser;
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService, ICurrentUser currentUser)
        {
            _currentUser = currentUser;
            _identityService = identityService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerHeader("request", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Registed User.",
            Description = "Registed a user with all its values set. This can only be done by the registered user",
            OperationId = "RegisterAsync")]
        [SwaggerResponse(201, "Return User Registered.")]
        [SwaggerResponse(400, "User name is in use or Phone number is registered")]
        [SwaggerResponse(500, "Identity Internal Server Error.")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _identityService.RegisterAsync(request, origin));
        }

        [HttpGet("user-picture/{userId}")]
        [AllowAnonymous]
        [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Client, Duration = 60)]
        [SwaggerHeader("userId", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Get Image User.",
            Description = "We get the picture associated to the user. This can only be done by the registered user",
            OperationId = "GetUserPictureAsync")]
        [SwaggerResponse(200, "Return picture user by id.")]
        [SwaggerResponse(404, "User was not found.")]
        public async Task<IActionResult> GetUserPictureAsync(string userId)
        {
            var response = await _identityService.GetUserPictureAsync(userId);
            return Ok(response);
        }

        [HttpPost("user-picture/{userId}")]
        [AllowAnonymous]
        [SwaggerHeader("request", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Update Image User.",
            Description = "We get the image user updated. This can only be done by the registered user",
            OperationId = "UpdateUserPictureAsync")]
        [SwaggerResponse(200, "Return updated image user.")]
        [SwaggerResponse(404, "User was not found.")]
        [SwaggerResponse(500, "Identity Internal Server Error.")]
        public async Task<IActionResult> UpdateUserPictureAsync(UpdateUserPictureRequest request)
        {
            string userId = _currentUser.GetUserId().ToString();
            var response = await _identityService.UpdateUserPictureAsync(request, userId);
            return Ok(response);
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        [SwaggerHeader("userId, code", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Confirm Email User.",
            Description = "We get the account email confirmed user. This can only be done by the registered user",
            OperationId = "ConfirmEmailAsync")]
        [SwaggerResponse(200, "Return email confirmed user.")]
        [SwaggerResponse(404, "User was not found.")]
        [SwaggerResponse(500, "An error occurred while confirming email.")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            var response = await _identityService.ConfirmEmailAsync(userId, code);
            return Ok(response);
        }

        [HttpGet("confirm-phone-number")]
        [AllowAnonymous]
        [SwaggerHeader("userId, code", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Confirm Phone Number User.",
            Description = "We get the phone number confirmed user. This can only be done by the registered user",
            OperationId = "ConfirmPhoneNumberAsync")]
        [SwaggerResponse(200, "Return phone number confirmed user.")]
        [SwaggerResponse(404, "User was not found.")]
        [SwaggerResponse(500, "An error occurred while confirming phone number.")]
        public async Task<IActionResult> ConfirmPhoneNumberAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _identityService.ConfirmPhoneNumberAsync(userId, code));
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [SwaggerHeader("request", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Forgot Password User.",
            Description = "We get the steps to follow to remember password. This can only be done by the registered user",
            OperationId = "ForgotPasswordAsync")]
        [SwaggerResponse(200, "Return forgot password user.")]
        [SwaggerResponse(404, "User was not found or Email not confirmed")]
        [SwaggerResponse(500, "Identity errors have occurred.")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _identityService.ForgotPasswordAsync(request, origin));
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [SwaggerHeader("request", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Forgot Password User.",
            Description = "We get the steps to follow to reset a new password. This can only be done by the registered user",
            OperationId = "ResetPasswordAsync")]
        [SwaggerResponse(200, "Return reset password user.")]
        [SwaggerResponse(404, "User was not found.")]
        [SwaggerResponse(500, "Identity errors have occurred.")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            return Ok(await _identityService.ResetPasswordAsync(request));
        }
    }
}