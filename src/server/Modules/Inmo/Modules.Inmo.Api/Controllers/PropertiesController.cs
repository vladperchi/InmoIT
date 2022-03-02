// --------------------------------------------------------------------------------------------------
// <copyright file="PropertiesController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Features.Properties.Commands;
using InmoIT.Modules.Inmo.Core.Features.Properties.Queries;
using InmoIT.Modules.Inmo.Core.Features.Properties.Queries.Export;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Dtos.Inmo.Properties;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Inmo.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class PropertiesController : BaseController
    {
        [HttpGet]
        [HavePermission(PermissionsConstant.Properties.ViewAll)]
        [SwaggerOperation(
            Summary = "Get Property List.",
            Description = "List all properties in the database. This can only be done by the registered user",
            OperationId = "GetAllAsync")]
        [SwaggerResponse(200, "Return property list.")]
        [SwaggerResponse(204, "Property list not content.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedPropertyFilter filter)
        {
            var request = Mapper.Map<GetAllPropertiesQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [HavePermission(PermissionsConstant.Properties.View)]
        [SwaggerOperation(
            Summary = "Get Property By Id.",
            Description = "We get the detail property by Id. This can only be done by the registered user",
            OperationId = "GetByIdAsync")]
        [SwaggerResponse(200, "Return owner by id.")]
        [SwaggerResponse(404, "Property was not found.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, Property> filter)
        {
            var request = Mapper.Map<GetPropertyByIdQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        [HavePermission(PermissionsConstant.Properties.Register)]
        [SwaggerOperation(
            Summary = "Registered Property.",
            Description = "Registed a property with all its values set. This can only be done by the registered user",
            OperationId = "RegisterAsync")]
        [SwaggerResponse(201, "Return registered property.")]
        [SwaggerResponse(400, "Property already exists.")]
        [SwaggerResponse(500, "Property Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> RegisterAsync(RegisterPropertyCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        [HavePermission(PermissionsConstant.Properties.Update)]
        [SwaggerOperation(
            Summary = "Update Property.",
            Description = "We get the property with its modified values.. This can only be done by the registered user",
            OperationId = "UpdateAsync")]
        [SwaggerResponse(200, "Return updated property.")]
        [SwaggerResponse(404, "Property was not found.")]
        [SwaggerResponse(500, "Property Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> UpdateAsync(UpdatePropertyCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        [HavePermission(PermissionsConstant.Properties.Remove)]
        [SwaggerOperation(
            Summary = "Remove Property.",
            Description = "We get the removed property by Id. This can only be done by the registered user",
            OperationId = "RemoveAsync")]
        [SwaggerResponse(200, "Return removed property.")]
        [SwaggerResponse(404, "Property was not found.")]
        [SwaggerResponse(500, "Property Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            var response = await Mediator.Send(new RemovePropertyCommand(id));
            return Ok(response);
        }

        [HttpGet("export")]
        [HavePermission(PermissionsConstant.Properties.Export)]
        [SwaggerOperation(
            Summary = "Export Properties To Excel.",
            Description = "We get an exported excel file of all properties. This can only be done by the registered user",
            OperationId = "ExportAsync")]
        [SwaggerResponse(200, "Return export properties to excel.")]
        [SwaggerResponse(204, "Property list not content.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> ExportAsync(string searchString = "") => Ok(await Mediator.Send(new ExportPropertiesQuery(searchString)));
    }
}