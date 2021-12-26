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
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [HavePermission(PermissionsConstant.Users.View)]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            return Ok(await _userService.GetByIdAsync(id));
        }

        [HttpPut]
        [HavePermission(PermissionsConstant.Users.Edit)]
        public async Task<IActionResult> UpdateAsync(UpdateUserRequest request)
        {
            return Ok(await _userService.UpdateAsync(request));
        }

        [HttpGet("roles/{id}")]
        [HavePermission(PermissionsConstant.Users.View)]
        public async Task<IActionResult> GetRolesAsync(string id)
        {
            return Ok(await _userService.GetRolesAsync(id));
        }

        [HttpPut("roles/{id}")]
        [HavePermission(PermissionsConstant.Users.Edit)]
        public async Task<IActionResult> UpdateUserRolesAsync(string id, UserRolesRequest request)
        {
            return Ok(await _userService.UpdateUserRolesAsync(id, request));
        }

        [HttpGet("export")]
        [HavePermission(PermissionsConstant.Users.Export)]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _userService.ExportToExcelAsync(searchString));
        }
    }
}