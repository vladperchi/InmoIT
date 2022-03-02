// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyTypesController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Commands;
using InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Queries;
using InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Queries.Export;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Dtos.Inmo.PropertyTypes;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Inmo.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class PropertyTypesController : BaseController
    {
        [HttpGet]
        [HavePermission(PermissionsConstant.PropertyTypes.ViewAll)]
        [SwaggerOperation(
            Summary = "Get Property Type List.",
            Description = "List all property types in the database. This can only be done by the registered user",
            OperationId = "GetAllAsync")]
        [SwaggerResponse(200, "Return property type list.")]
        [SwaggerResponse(204, "Property type list not content.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedPropertyTypeFilter filter)
        {
            var request = Mapper.Map<GetAllPropertyTypesQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [HavePermission(PermissionsConstant.PropertyTypes.View)]
        [SwaggerOperation(
            Summary = "Get Property Type By Id.",
            Description = "We get the detail property type by Id. This can only be done by the registered user",
            OperationId = "GetByIdAsync")]
        [SwaggerResponse(200, "Return property type by id.")]
        [SwaggerResponse(404, "Property Type was not found.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, PropertyType> filter)
        {
            var request = Mapper.Map<GetPropertyTypeByIdQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        [HavePermission(PermissionsConstant.PropertyTypes.Create)]
        [SwaggerOperation(
            Summary = "Created Property Type List.",
            Description = "Created a property type list with all its values set. This can only be done by the registered user",
            OperationId = "CreateAsync")]
        [SwaggerResponse(201, "Return added property image list.")]
        [SwaggerResponse(400, "Property Type already exists.")]
        [SwaggerResponse(500, "Property Type Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> CreateAsync(CreatePropertyTypeCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        [HavePermission(PermissionsConstant.PropertyTypes.Update)]
        [SwaggerOperation(
            Summary = "Update Property Type.",
            Description = "We get the property type with its modified values. This can only be done by the registered user",
            OperationId = "UpdateAsync")]
        [SwaggerResponse(200, "Return updated property image.")]
        [SwaggerResponse(404, "Property Type was not found.")]
        [SwaggerResponse(500, "Property Type Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> UpdateAsync(UpdatePropertyTypeCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("{propertyTypeId:guid}")]
        [HavePermission(PermissionsConstant.PropertyTypes.View)]
        [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Client, Duration = 60)]
        [SwaggerOperation(
            Summary = "Get Image Property Type.",
            Description = "We get the image associated to the property type. This can only be done by the registered user",
            OperationId = "GetPictureAsync")]
        [SwaggerResponse(200, "Return image Property Type by id.")]
        [SwaggerResponse(404, "Property Type was not found.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetPictureAsync(GetPropertyTypeImageQuery request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        [HavePermission(PermissionsConstant.PropertyTypes.Remove)]
        [SwaggerOperation(
            Summary = "Remove Property Type.",
            Description = "We get the removed property type by Id. This can only be done by the registered user",
            OperationId = "RemoveAsync")]
        [SwaggerResponse(200, "Return removed property type.")]
        [SwaggerResponse(404, "Property Image was not found.")]
        [SwaggerResponse(405, "Not allowed to deletion.")]
        [SwaggerResponse(500, "Property Image Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            var response = await Mediator.Send(new RemovePropertyTypeCommand(id));
            return Ok(response);
        }

        [HttpGet("export")]
        [HavePermission(PermissionsConstant.PropertyTypes.Export)]
        [SwaggerOperation(
            Summary = "Export Property Types To Excel.",
            Description = "We get an exported excel file of all property. This can only be done by the registered user",
            OperationId = "ExportAsync")]
        [SwaggerResponse(200, "Return export property types to excel.")]
        [SwaggerResponse(404, "Property Types was not found.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> ExportAsync(string searchString = "") => Ok(await Mediator.Send(new ExportPropertyTypesQuery(searchString)));
    }
}