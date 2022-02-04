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
using Swashbuckle.AspNetCore.Annotations;
using InmoIT.Shared.Core.Attributes;
using Microsoft.AspNetCore.Http;

namespace InmoIT.Modules.Identity.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class RolesController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IRoleClaimService _roleClaimService;

        public RolesController(
            IRoleService roleService,
            IRoleClaimService roleClaimService)
        {
            _roleService = roleService;
            _roleClaimService = roleClaimService;
        }

        /// <response code="200">Return role list.</response>
        /// <response code="204">Role list not content.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpGet]
        [HavePermission(PermissionsConstant.Roles.View)]
        [SwaggerOperation(Summary = "Get Role List.", Description = "SuperAdmin, Admin, Basic and more.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _roleService.GetAllAsync();
            return Ok(response);
        }

        /// <response code="200">Return role by id.</response>
        /// <response code="404">Role not found.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpGet("{id}")]
        [HavePermission(PermissionsConstant.Roles.View)]
        [SwaggerHeader("id", "Input data required", "", true)]
        [SwaggerOperation(Summary = "Get Role By Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var response = await _roleService.GetByIdAsync(id);
            return Ok(response);
        }

        /// <response code="200">Return add or update rol.</response>
        /// <response code="400">Role exists.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpPost]
        [HavePermission(PermissionsConstant.Roles.Create)]
        [SwaggerHeader("request", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Add Or Update Rol.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddOrUpdateAsync(RoleRequest request)
        {
            var response = await _roleService.SaveAsync(request);
            return Ok(response);
        }

        /// <response code="200">Return deleted rol.</response>
        /// <response code="404">Role not found.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpDelete("{id}")]
        [HavePermission(PermissionsConstant.Roles.Delete)]
        [SwaggerHeader("id", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Deleted Rol.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var response = await _roleService.DeleteAsync(id);
            return Ok(response);
        }

        /// <response code="200">Return list permission by rol.</response>
        /// <response code="404">Role not exists.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpGet("permissions/byrole/{roleId}")]
        [HavePermission(PermissionsConstant.RoleClaims.View)]
        [SwaggerHeader("roleId", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Get List Permission By Rol Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPermissionsByRoleIdAsync([FromRoute] string roleId)
        {
            var response = await _roleClaimService.GetAllPermissionsAsync(roleId);
            return Ok(response);
        }

        /// <response code="200">Return role claim list.</response>
        /// <response code="204">Role claim list not content.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpGet("permissions")]
        [HavePermission(PermissionsConstant.RoleClaims.View)]
        [SwaggerOperation(Summary = "Get Role Claim List.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllClaimsAsync()
        {
            var response = await _roleClaimService.GetAllAsync();
            return Ok(response);
        }

        /// <response code="200">Return role claims by id.</response>
        /// <response code="404">Role Claim not found.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpGet("permissions/{id}")]
        [HavePermission(PermissionsConstant.RoleClaims.View)]
        [SwaggerHeader("id", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Get Role Claims By Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetClaimByIdAsync([FromRoute] int id)
        {
            var response = await _roleClaimService.GetByIdAsync(id);
            return Ok(response);
        }

        /// <response code="200">Return edit a role claims.</response>
        /// <response code="404">Role does not exist.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpPut("permissions/update")]
        [HavePermission(PermissionsConstant.RoleClaims.Edit)]
        [SwaggerHeader("request", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Role Claims Updated.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdatePermissionsAsync(PermissionRequest request)
        {
            var response = await _roleClaimService.UpdatePermissionsAsync(request);
            return Ok(response);
        }

        /// <response code="200">Return deleted role claims by id.</response>
        /// <response code="404">Role Claim does not exist.</response>
        /// <response code="401">Without authorization to access.</response>
        /// <response code="403">No permission to access.</response>
        [HttpDelete("permissions/{id}")]
        [HavePermission(PermissionsConstant.RoleClaims.Delete)]
        [SwaggerHeader("id", "Input data required in API", "", true)]
        [SwaggerOperation(Summary = "Deleted Role Claims By Id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteClaimByIdAsync([FromRoute] int id)
        {
            var response = await _roleClaimService.DeleteAsync(id);
            return Ok(response);
        }
    }
}