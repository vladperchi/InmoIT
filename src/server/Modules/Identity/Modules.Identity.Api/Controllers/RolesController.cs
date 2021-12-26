// --------------------------------------------------------------------------------------------------
// <copyright file="RolesController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Shared.Infrastructure.Permissions;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Dtos.Identity.Roles;
using Microsoft.AspNetCore.Mvc;

namespace InmoIT.Modules.Identity.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class RolesController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IRoleClaimService _roleClaimService;

        public RolesController(IRoleService roleService, IRoleClaimService roleClaimService)
        {
            _roleService = roleService;
            _roleClaimService = roleClaimService;
        }

        [HttpGet]
        [HavePermission(PermissionsConstant.Roles.View)]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _roleService.GetAllAsync());
        }

        [HttpPost]
        [HavePermission(PermissionsConstant.Roles.Create)]
        public async Task<IActionResult> PostAsync(RoleRequest request)
        {
            return Ok(await _roleService.SaveAsync(request));
        }

        [HttpDelete("{id}")]
        [HavePermission(PermissionsConstant.Roles.Delete)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            return Ok(await _roleService.DeleteAsync(id));
        }

        [HttpGet("permissions/byrole/{roleId}")]
        [HavePermission(PermissionsConstant.RoleClaims.View)]
        public async Task<IActionResult> GetPermissionsByRoleIdAsync([FromRoute] string roleId)
        {
            return Ok(await _roleClaimService.GetAllPermissionsAsync(roleId));
        }

        [HttpGet("permissions")]
        [HavePermission(PermissionsConstant.RoleClaims.View)]
        public async Task<IActionResult> GetAllClaimsAsync()
        {
            return Ok(await _roleClaimService.GetAllAsync());
        }

        [HttpGet("permissions/{id}")]
        [HavePermission(PermissionsConstant.RoleClaims.View)]
        public async Task<IActionResult> GetClaimByIdAsync([FromRoute] int id)
        {
            return Ok(await _roleClaimService.GetByIdAsync(id));
        }

        [HttpPut("permissions/update")]
        [HavePermission(PermissionsConstant.RoleClaims.Edit)]
        public async Task<IActionResult> UpdatePermissionsAsync(PermissionRequest request)
        {
            return Ok(await _roleClaimService.UpdatePermissionsAsync(request));
        }

        [HttpDelete("permissions/{id}")]
        [HavePermission(PermissionsConstant.RoleClaims.Delete)]
        public async Task<IActionResult> DeleteClaimByIdAsync([FromRoute] int id)
        {
            return Ok(await _roleClaimService.DeleteAsync(id));
        }
    }
}