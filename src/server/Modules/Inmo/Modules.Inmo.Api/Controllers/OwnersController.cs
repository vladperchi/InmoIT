// --------------------------------------------------------------------------------------------------
// <copyright file="OwnersController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Features.Owners.Commands;
using InmoIT.Modules.Inmo.Core.Features.Owners.Queries;
using InmoIT.Modules.Inmo.Core.Features.Owners.Queries.Export;
using InmoIT.Shared.Core.Attributes;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Dtos.Inmo.Owners;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Inmo.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class OwnersController : BaseController
    {
        [HttpGet]
        [HavePermission(PermissionsConstant.Owners.ViewAll)]
        [SwaggerHeader("filter", "Input data not required", "", false)]
        [SwaggerOperation(
            Summary = "Get Owner List.",
            Description = "List all owners in the database. This can only be done by the registered user",
            OperationId = "GetAllAsync")]
        [SwaggerResponse(200, "Return owner list.")]
        [SwaggerResponse(204, "Owner list not content.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedOwnerFilter filter)
        {
            var request = Mapper.Map<GetAllOwnersQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [HavePermission(PermissionsConstant.Owners.View)]
        [SwaggerHeader("filter", "Input data not required", "", true)]
        [SwaggerOperation(
            Summary = "Get Owner By Id.",
            Description = "We get the detail owner by Id. This can only be done by the registered user",
            OperationId = "GetByIdAsync")]
        [SwaggerResponse(200, "Return owner by id.")]
        [SwaggerResponse(404, "Owner was not found.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, Owner> filter)
        {
            var request = Mapper.Map<GetOwnerByIdQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{ownerId:guid}")]
        [HavePermission(PermissionsConstant.Owners.View)]
        [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Client, Duration = 60)]
        [SwaggerHeader("request", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Get Picture Owner.",
            Description = "We get the picture associated to the owner. This can only be done by the registered user",
            OperationId = "GetPictureAsync")]
        [SwaggerResponse(200, "Return picture owner by id.")]
        [SwaggerResponse(404, "Owner was not found.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetPictureAsync(GetOwnerImageQuery request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        [HavePermission(PermissionsConstant.Owners.Register)]
        [SwaggerHeader("command", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Registered Owner.",
            Description = "Registed a owner with all its values set. This can only be done by the registered user",
            OperationId = "RegisterAsync")]
        [SwaggerResponse(201, "Return registered owner.")]
        [SwaggerResponse(400, "Owner already exists.")]
        [SwaggerResponse(500, "Owner Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> RegisterAsync(RegisterOwnerCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        [HavePermission(PermissionsConstant.Owners.Update)]
        [SwaggerHeader("command", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Update Owner.",
            Description = "We get the owner with its modified values. This can only be done by the registered user",
            OperationId = "UpdateAsync")]
        [SwaggerResponse(200, "Return updated owner.")]
        [SwaggerResponse(404, "Owner was not found.")]
        [SwaggerResponse(500, "Owner Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> UpdateAsync(UpdateOwnerCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        [HavePermission(PermissionsConstant.Owners.Remove)]
        [SwaggerHeader("id", "Input data required", "", true)]
        [SwaggerOperation(
            Summary = "Remove Owner.",
            Description = "We get the removed owner by Id. This can only be done by the registered user",
            OperationId = "RemoveAsync")]
        [SwaggerResponse(200, "Return removed owner.")]
        [SwaggerResponse(404, "Owner was not found.")]
        [SwaggerResponse(500, "Owner Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            var response = await Mediator.Send(new RemoveOwnerCommand(id));
            return Ok(response);
        }

        [HttpGet("export")]
        [HavePermission(PermissionsConstant.Owners.Export)]
        [SwaggerHeader("searchString", "Input data required", "", false)]
        [SwaggerOperation(
            Summary = "Export Owners To Excel.",
            Description = "We get an exported excel file of all owners. This can only be done by the registered user",
            OperationId = "ExportAsync")]
        [SwaggerResponse(200, "Return export customers to excel.")]
        [SwaggerResponse(204, "Owner list not content.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> ExportAsync(string searchString = "") => Ok(await Mediator.Send(new ExportOwnersQuery(searchString)));
    }
}