// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyImageService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

using InmoIT.Modules.Inmo.Core.Features.Images.Queries;
using InmoIT.Shared.Core.Integration.Inmo;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.Images;
using InmoIT.Shared.Infrastructure.Common;

using MediatR;

namespace InmoIT.Modules.Inmo.Infrastructure.Services
{
    public class PropertyImageService : IPropertyImageService
    {
        private readonly IMediator _mediator;

        public PropertyImageService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<GetPropertyImageByPropertyIdResponse>> GetDetailsPropertyImageAsync(Guid propertyId)
        {
            return await _mediator.Send(new GetImageByPropertyIdQuery(propertyId));
        }

        public async Task<string> GenerateFileName(int length)
        {
            return await Utilities.GenerateCode("P", length);
        }
    }
}