// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyImagesController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Features.Images.Commands;
using InmoIT.Modules.Inmo.Core.Features.Images.Queries;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Dtos.Inmo.Images;
using InmoIT.Shared.Infrastructure.Permissions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InmoIT.Modules.Inmo.Api.Controllers
{
    [ApiVersion("1")]
    internal sealed class PropertyImagesController : BaseController
    {
        [HttpGet]
        [HavePermission(PermissionsConstant.Images.ViewAll)]
        [SwaggerOperation(
            Summary = "Get Property Image List.",
            Description = "List all property images in the database. This can only be done by the registered user",
            OperationId = "GetAllAsync")]
        [SwaggerResponse(200, "Return property image list.")]
        [SwaggerResponse(204, "Property image list not content.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginatedPropertyImageFilter filter)
        {
            var request = Mapper.Map<GetAllImagesQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [HavePermission(PermissionsConstant.Images.View)]
        [SwaggerOperation(
            Summary = "Get Property Image By Id.",
            Description = "We get the detail property image by Id. This can only be done by the registered user",
            OperationId = "GetByIdAsync")]
        [SwaggerResponse(200, "Return property image by id.")]
        [SwaggerResponse(404, "Property Image was not found.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdCacheableFilter<Guid, PropertyImage> filter)
        {
            var request = Mapper.Map<GetImageByPropertyIdQuery>(filter);
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        [HavePermission(PermissionsConstant.Images.Add)]
        [SwaggerOperation(
            Summary = "Added Property Image List.",
            Description = "Added a property image list with all its values set. This can only be done by the registered user",
            OperationId = "AddAsync")]
        [SwaggerResponse(201, "Return added property image list.")]
        [SwaggerResponse(404, "Property was not found.")]
        [SwaggerResponse(400, "Property Image already exists.")]
        [SwaggerResponse(500, "Property Image Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> AddAsync(AddImageCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        [HavePermission(PermissionsConstant.Images.Edit)]
        [SwaggerOperation(
            Summary = "Edit Property Image.",
            Description = "We get the property image with its modified values. This can only be done by the registered user",
            OperationId = "UpdateAsync")]
        [SwaggerResponse(200, "Return updated property image.")]
        [SwaggerResponse(404, "Property Image was not found.")]
        [SwaggerResponse(500, "Property Image Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> EditAsync(UpdateImageCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        [HavePermission(PermissionsConstant.Images.Remove)]
        [SwaggerOperation(
            Summary = "Remove Property Image.",
            Description = "We get the removed property image by Id. This can only be done by the registered user",
            OperationId = "RemoveAsync")]
        [SwaggerResponse(200, "Return removed property image.")]
        [SwaggerResponse(404, "Property Image was not found.")]
        [SwaggerResponse(500, "Property Image Internal Server Error.")]
        [SwaggerResponse(401, "No authorization to access.")]
        [SwaggerResponse(403, "No permission to access.")]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            var response = await Mediator.Send(new RemoveImageCommand(id));
            return Ok(response);
        }
    }
}