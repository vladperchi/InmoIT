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

        [HttpGet]
        [HavePermission(PermissionsConstant.Roles.ViewAll)]
        [SwaggerOperation(
            Summary = "Get Role List.",
            Description = "List all roles in the database. This can only be done by the registered user",
            OperationId = "GetAllAsync")]
        [SwaggerResponse(200, "Return role list.")]
        [SwaggerResponse(204, "Role list not content.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _roleService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [HavePermission(PermissionsConstant.Roles.View)]
        [SwaggerHeader("id", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Get Role By Id.",
            Description = "We get the detail role by Id. This can only be done by the registered user",
            OperationId = "GetByIdAsync")]
        [SwaggerResponse(200, "Return role by id.")]
        [SwaggerResponse(404, "Role was not found.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var response = await _roleService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        [HavePermission(PermissionsConstant.Roles.AddOrUpdate)]
        [SwaggerHeader("request", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Add Or Update Rol.",
            Description = "We get the role with its added or modified values. This can only be done by the registered user",
            OperationId = "AddOrUpdateAsync")]
        [SwaggerResponse(200, "Return added or updated rol.")]
        [SwaggerResponse(400, "Rol already exists.")]
        [SwaggerResponse(500, "Rol Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> AddOrUpdateAsync(RoleRequest request)
        {
            var response = await _roleService.AddOrUpdateAsync(request);
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        [HavePermission(PermissionsConstant.Roles.Delete)]
        [SwaggerHeader("id", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Delete Rol.",
            Description = "We get the deleted rol by Id database. This can only be done by the registered user",
            OperationId = "DeleteAsync")]
        [SwaggerResponse(200, "Return deleted rol.")]
        [SwaggerResponse(404, "Rol was not found.")]
        [SwaggerResponse(500, "Not allowed to delete Role.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var response = await _roleService.DeleteAsync(id);
            return Ok(response);
        }

        [HttpGet("permissions/byrole/{roleId}")]
        [HavePermission(PermissionsConstant.RoleClaims.View)]
        [SwaggerHeader("roleId", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Get Permission List By Rol Id.",
            Description = "We get the permission list by rol id. This can only be done by the registered user",
            OperationId = "GetPermissionsByRoleIdAsync")]
        [SwaggerResponse(200, "Return permission list by rol id.")]
        [SwaggerResponse(404, "Rol was not found.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetPermissionsByRoleIdAsync([FromRoute] string roleId)
        {
            var response = await _roleClaimService.GetAllPermissionsAsync(roleId);
            return Ok(response);
        }

        [HttpGet("permissions")]
        [HavePermission(PermissionsConstant.RoleClaims.View)]
        [SwaggerOperation(
            Summary = "Get Role Claim List.",
            Description = "List all role claim in the database. This can only be done by the registered user",
            OperationId = "GetAllClaimsAsync")]
        [SwaggerResponse(200, "Return role claim list.")]
        [SwaggerResponse(204, "Role Claim list not content.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetAllClaimsAsync()
        {
            var response = await _roleClaimService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("permissions/{id}")]
        [HavePermission(PermissionsConstant.RoleClaims.View)]
        [SwaggerHeader("id", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Get Role Claims By Id.",
            Description = "We get the detail role claim by id. This can only be done by the registered user",
            OperationId = "GetClaimByIdAsync")]
        [SwaggerResponse(200, "Return role claim list.")]
        [SwaggerResponse(404, "Role Claim not found.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetClaimByIdAsync([FromRoute] int id)
        {
            var response = await _roleClaimService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpPut("permissions/update")]
        [HavePermission(PermissionsConstant.RoleClaims.Edit)]
        [SwaggerHeader("request", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Get Updated Role Claim.",
            Description = "We get the role claim with its modified values.. This can only be done by the registered user",
            OperationId = "UpdatePermissionsAsync")]
        [SwaggerResponse(200, "Return updated role claim.")]
        [SwaggerResponse(404, "Role was not found.")]
        [SwaggerResponse(500, "Not allowed to modify Permissions.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> UpdatePermissionsAsync(PermissionRequest request)
        {
            var response = await _roleClaimService.UpdatePermissionsAsync(request);
            return Ok(response);
        }

        [HttpDelete("permissions/{id}")]
        [HavePermission(PermissionsConstant.RoleClaims.Delete)]
        [SwaggerHeader("id", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Deleted Role Claims By Id.",
            Description = "We get the deleted role claims by Id. This can only be done by the registered user",
            OperationId = "RemoveAsync")]
        [SwaggerResponse(200, "Return removed role claims.")]
        [SwaggerResponse(404, "Rol Claim was not found.")]
        [SwaggerResponse(500, "Not allowed to delete Rol Claim.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> DeleteClaimByIdAsync([FromRoute] int id)
        {
            var response = await _roleClaimService.DeleteAsync(id);
            return Ok(response);
        }
    }
}