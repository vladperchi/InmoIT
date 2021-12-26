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

namespace InmoIT.Modules.Identity.Api.Controllers
{
    [ApiVersion("1")]
    [Route(BasePath)]
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
        [HavePermission(PermissionsConstant.Identity.Register)]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _identityService.RegisterAsync(request, origin));
        }

        [HttpGet("user-picture/{userId}")]
        [AllowAnonymous]
        [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Client, Duration = 60)]
        public async Task<IActionResult> GetUserPictureAsync(string userId)
        {
            return Ok(await _identityService.GetUserPictureAsync(userId));
        }

        [HttpPost("user-picture/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUserPictureAsync(UpdateUserPictureRequest request)
        {
            string userId = _currentUser.GetUserId().ToString();
            return Ok(await _identityService.UpdateUserPictureAsync(request, userId));
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _identityService.ConfirmEmailAsync(userId, code));
        }

        [HttpGet("confirm-phone-number")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmPhoneNumberAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _identityService.ConfirmPhoneNumberAsync(userId, code));
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _identityService.ForgotPasswordAsync(request, origin));
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            return Ok(await _identityService.ResetPasswordAsync(request));
        }
    }
}