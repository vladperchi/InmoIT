// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using InmoIT.Modules.Inmo.Core.Abstractions;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Exceptions;
using InmoIT.Modules.Inmo.Core.Features.Properties.Commands;
using InmoIT.Modules.Inmo.Core.Features.Properties.Events;
using InmoIT.Modules.Inmo.Core.Features.Properties.Queries;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Integration.Inmo;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.Properties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using static InmoIT.Shared.Core.Constants.PermissionsConstant;

namespace InmoIT.Modules.Inmo.Infrastructure.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IDistributedCache _cache;
        private readonly IInmoDbContext _context;
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<PropertyService> _localizer;
        private readonly ILogger<PropertyService> _logger;

        public PropertyService(
            IDistributedCache cache,
            IInmoDbContext context,
            IMediator mediator,
            IStringLocalizer<PropertyService> localizer,
            ILogger<PropertyService> logger)
        {
            _cache = cache;
            _context = context;
            _mediator = mediator;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<Result<GetPropertyByIdResponse>> GetDetailsPropertyAsync(Guid propertyId)
        {
            return await _mediator.Send(new GetPropertyByIdQuery(propertyId, false));
        }

        public async Task<Result<Guid>> RemovePropertyAsync(Guid propertyId)
        {
            return await _mediator.Send(new RemovePropertyCommand(propertyId));
        }

        public async Task<bool> IsPropertyTypeUsed(Guid propertyTypeId)
        {
            return await _context.Properties.AnyAsync(x => x.PropertyTypeId == propertyTypeId);
        }

        public async Task<int> GetCountAsync() => await _context.Properties.CountAsync();

        public async Task<Result<Guid>> ChangeStatusPropertyAsync(Guid propertyId, bool status)
{
            var property = await _context.Properties.Where(x => x.Id == propertyId).FirstOrDefaultAsync();
            _ = property ?? throw new PropertyNotFoundException(_localizer, propertyId);
            try
            {
                property.IsActive = status;
                string statusProperty = property.IsActive ? "Active" : "Inactive";
                _context.Properties.Update(property);
                property.AddDomainEvent(new PropertyUpdatedEvent(property));
                await _context.SaveChangesAsync();
                await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, Property>(property.Id));
                _logger.LogInformation(string.Format(_localizer["Status Property: {0} with Id: {1}"], statusProperty, property.Id));
                return await Result<Guid>.SuccessAsync(property.Id, _localizer[$"Property {statusProperty}"]);
            }
            catch (Exception)
            {
                throw new PropertyCustomException(_localizer, null);
            }
        }
    }
}