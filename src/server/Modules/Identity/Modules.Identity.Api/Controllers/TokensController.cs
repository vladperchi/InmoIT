// --------------------------------------------------------------------------------------------------
// <copyright file="TokensController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using InmoIT.Shared.Core.Attributes;
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

        public TokensController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerHeader("request", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Get Token.", Description = "Submit Credentials to generate valid Access Token.")]
        [SwaggerResponse(200, "Return token user.")]
        [SwaggerResponse(500, "Internal Server Error.")]
        [SwaggerResponse(401, "Without authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetTokenAsync(TokenRequest request)
        {
            var response = await _tokenService.GetTokenAsync(request, IPAddressGenerate());
            return Ok(response);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [SwaggerHeader("request", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Refresh Token.", Description = "Recovery of the access token every time it expires.")]
        [SwaggerResponse(200, "Return recovery token user.")]
        [SwaggerResponse(500, "Internal Server Error.")]
        [SwaggerResponse(401, "Without authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RefreshAsync(RefreshTokenRequest request)
        {
            var response = await _tokenService.RefreshTokenAsync(request, IPAddressGenerate());
            return Ok(response);
        }

        private string IPAddressGenerate()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            }
        }
    }
}