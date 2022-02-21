// --------------------------------------------------------------------------------------------------
// <copyright file="TokensController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using InmoIT.Shared.Core.Interfaces.Services.Identity;
using InmoIT.Shared.Dtos.Identity.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Identity.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class TokensController : BaseController
    {
        private readonly ITokenService _tokenService;

        public TokensController(
            ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Get Token.",
            Description = "Submit Credentials to generate valid Access Token.",
            OperationId = "GetTokenAsync")]
        [SwaggerResponse(200, "Return token user.")]
        [SwaggerResponse(404, "User was not found.")]
        public async Task<IActionResult> GetTokenAsync(TokenRequest request)
        {
            var response = await _tokenService.GetTokenAsync(request, IPAddressGenerate());
            return Ok(response);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Refresh Token.",
            Description = "Recovery of the access token every time it expires.",
            OperationId = "RefreshTokenAsync")]
        [SwaggerResponse(200, "Return recovery token user.")]
        [SwaggerResponse(404, "User was not found.")]
        public async Task<ActionResult> RefreshAsync(RefreshTokenRequest request)
        {
            var response = await _tokenService.RefreshTokenAsync(request, IPAddressGenerate());
            return Ok(response);
        }

        private string IPAddressGenerate() => Request.Headers.ContainsKey("X-Forwarded-For")
            ? Request.Headers["X-Forwarded-For"]
            : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
    }
}