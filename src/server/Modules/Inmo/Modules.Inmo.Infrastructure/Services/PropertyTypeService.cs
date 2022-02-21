// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyTypeService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Inmo.Core.Abstractions;
using InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Commands;
using InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Queries;
using InmoIT.Shared.Core.Integration.Inmo;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.PropertyTypes;
using InmoIT.Shared.Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Inmo.Infrastructure.Services
{
    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly IInmoDbContext _context;
        private readonly IMediator _mediator;

        public PropertyTypeService(
            IInmoDbContext context,
            IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Result<GetPropertyTypeByIdResponse>> GetDetailsPropertyTypeAsync(Guid propertyTypeId)
        {
            return await _mediator.Send(new GetPropertyTypeByIdQuery(propertyTypeId, false));
        }

        public async Task<Result<Guid>> RemovePropertyTypeAsync(Guid propertyTypeId)
        {
            return await _mediator.Send(new RemovePropertyTypeCommand(propertyTypeId));
        }

        public async Task<string> GenerateFileName(int length) => await Utilities.GenerateCode("PT", length);

        public async Task<int> GetCountAsync() => await _context.PropertyTypes.CountAsync();
    }
}