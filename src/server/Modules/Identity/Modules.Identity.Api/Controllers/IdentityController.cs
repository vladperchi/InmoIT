// --------------------------------------------------------------------------------------------------
// <copyright file="IdentityController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Shared.Infrastructure.Permissions;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Interfaces.Services.Identity;
using InmoIT.Shared.Dtos.Identity.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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

        /// <response code="201">Return created user.</response>
        /// <response code="400">User is null.</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerHeader("request", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Created User.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _identityService.RegisterAsync(request, origin));
        }

        /// <response code="200">Return image user by id.</response>
        /// <response code="404">User was not found.</response>
        [HttpGet("user-picture/{userId}")]
        [AllowAnonymous]
        [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Client, Duration = 60)]
        [SwaggerHeader("userId", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Get Image User.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserPictureAsync(string userId)
        {
            var response = await _identityService.GetUserPictureAsync(userId);
            return Ok(response);
        }

        /// <response code="200">Return updated image user.</response>
        /// <response code="404">User was not found.</response>
        [HttpPost("user-picture/{userId}")]
        [AllowAnonymous]
        [SwaggerHeader("FileName, Extension, UploadStorageType, Data", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Update Image User.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserPictureAsync(UpdateUserPictureRequest request)
        {
            string userId = _currentUser.GetUserId().ToString();
            var response = await _identityService.UpdateUserPictureAsync(request, userId);
            return Ok(response);
        }

        /// <response code="200">Return account email confirmed user.</response>
        /// <response code="404">User was not found.</response>
        [HttpGet("confirm-email")]
        [AllowAnonymous]
        [SwaggerHeader("userId, code", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Confirm Email User.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            var response = await _identityService.ConfirmEmailAsync(userId, code);
            return Ok(response);
        }

        /// <response code="200">Return account confirmed for phone Number user.</response>
        /// <response code="404">User was not found.</response>
        [HttpGet("confirm-phone-number")]
        [AllowAnonymous]
        [SwaggerHeader("userId, code", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Confirm Phone Number User.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmPhoneNumberAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _identityService.ConfirmPhoneNumberAsync(userId, code));
        }

        /// <response code="200">Return forgot password user.</response>
        /// <response code="500">Identity errors have occurred.</response>
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [SwaggerHeader("Email, Password, Token", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Forgot Password User.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _identityService.ForgotPasswordAsync(request, origin));
        }

        /// <response code="200">Return reset password user.</response>
        /// <response code="500">Identity errors have occurred.</response>
        [HttpPost("reset-password")]
        [AllowAnonymous]
        [SwaggerHeader("Email, Password, Token", "Input data required to validate in API", "", true)]
        [SwaggerOperation(Summary = "Reset Password User.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            return Ok(await _identityService.ResetPasswordAsync(request));
        }
    }
}