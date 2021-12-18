// --------------------------------------------------------------------------------------------------
// <copyright file="RolesController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Modules.Identity.Infrastructure.Permissions;
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

        /// <summary>
        /// Get All Roles (basic, admin etc.)
        /// </summary>
        /// <returns>Status 200 OK.</returns>
        [HttpGet]
        [HavePermission(PermissionsConstant.Roles.View)]
        public async Task<IActionResult> GetAllAsync()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        /// <summary>
        /// Add a Role.
        /// </summary>
        /// <returns>Status 200 OK.</returns>
        [HttpPost]
        [HavePermission(PermissionsConstant.Roles.Create)]
        public async Task<IActionResult> PostAsync(RoleRequest request)
        {
            var response = await _roleService.SaveAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Delete a Role.
        /// </summary>
        /// <returns>Status 200 OK.</returns>
        [HttpDelete("{id}")]
        [HavePermission(PermissionsConstant.Roles.Delete)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var response = await _roleService.DeleteAsync(id);
            return Ok(response);
        }

        /// <summary>
        /// Get Permissions By Role Id.
        /// </summary>
        /// <returns>Status 200 Ok.</returns>
        [HttpGet("permissions/byrole/{roleId}")]
        [HavePermission(PermissionsConstant.RoleClaims.View)]
        public async Task<IActionResult> GetPermissionsByRoleIdAsync([FromRoute] string roleId)
        {
            var response = await _roleClaimService.GetAllPermissionsAsync(roleId);
            return Ok(response);
        }

        /// <summary>
        /// Get All Role Claims.
        /// </summary>
        /// <returns>Status 200 Ok.</returns>
        [HttpGet("permissions")]
        [HavePermission(PermissionsConstant.RoleClaims.View)]
        public async Task<IActionResult> GetAllClaimsAsync()
        {
            var response = await _roleClaimService.GetAllAsync();
            return Ok(response);
        }

        /// <summary>
        /// Get a Role Claim By Id.
        /// </summary>
        /// <returns>Status 200 Ok.</returns>
        [HttpGet("permissions/{id}")]
        [HavePermission(PermissionsConstant.RoleClaims.View)]
        public async Task<IActionResult> GetClaimByIdAsync([FromRoute] int id)
        {
            var response = await _roleClaimService.GetByIdAsync(id);
            return Ok(response);
        }

        /// <summary>
        /// Edit a Role Claims.
        /// </summary>
        [HttpPut("permissions/update")]
        [HavePermission(PermissionsConstant.RoleClaims.Edit)]
        public async Task<IActionResult> UpdatePermissionsAsync(PermissionRequest request)
        {
            var response = await _roleClaimService.UpdatePermissionsAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Delete a Role Claim By Id.
        /// </summary>
        /// <returns>Status 200 Ok.</returns>
        [HttpDelete("permissions/{id}")]
        [HavePermission(PermissionsConstant.RoleClaims.Delete)]
        public async Task<IActionResult> DeleteClaimByIdAsync([FromRoute] int id)
        {
            var response = await _roleClaimService.DeleteAsync(id);
            return Ok(response);
        }
    }
}