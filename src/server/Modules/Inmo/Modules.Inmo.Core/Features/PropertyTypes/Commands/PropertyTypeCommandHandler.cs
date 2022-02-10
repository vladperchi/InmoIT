// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyTypeCommandHandler.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InmoIT.Modules.Inmo.Core.Abstractions;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Exceptions;
using InmoIT.Modules.Inmo.Core.Features.Owners.Events;
using InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Events;
using InmoIT.Shared.Core.Common.Enums;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Integration.Inmo;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Upload;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Commands
{
    internal class PropertyTypeCommandHandler :
        IRequestHandler<CreatePropertyTypeCommand, Result<Guid>>,
        IRequestHandler<UpdatePropertyTypeCommand, Result<Guid>>,
        IRequestHandler<RemovePropertyTypeCommand, Result<Guid>>
    {
        private readonly IDistributedCache _cache;
        private readonly IInmoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<PropertyTypeCommandHandler> _localizer;
        private readonly IPropertyTypeService _propertyTypeService;

        public PropertyTypeCommandHandler(
            IInmoDbContext context,
            IMapper mapper,
            IUploadService uploadService,
            IStringLocalizer<PropertyTypeCommandHandler> localizer,
            IPropertyTypeService propertyTypeService,
            IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _propertyTypeService = propertyTypeService;
            _cache = cache;
        }

        public async Task<Result<Guid>> Handle(CreatePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            if (await _context.PropertyTypes
                .Where(x => x.IsActive)
                .AnyAsync(x => x.Name == command.Name, cancellationToken))
            {
                throw new PropertyTypeAlreadyExistsException(_localizer);
            }

            var propertyType = _mapper.Map<PropertyType>(command);
            if (command.FileUploadRequest != null)
            {
                var fileUploadRequest = new FileUploadRequest
                {
                    Data = command.FileUploadRequest?.Data,
                    Extension = Path.GetExtension(command.FileUploadRequest.FileName),
                    UploadStorageType = UploadStorageType.PropertyType
                };
                string fileName = await _propertyTypeService.GenerateFileName(10);
                fileUploadRequest.FileName = $"{fileName}.{fileUploadRequest.Extension}";
                propertyType.ImageUrl = await _uploadService.UploadAsync(fileUploadRequest, FileType.Image);
            }

            propertyType.IsActive = true;
            try
            {
                propertyType.AddDomainEvent(new PropertyTypeRegisteredEvent(propertyType));
                await _context.PropertyTypes.AddAsync(propertyType, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<Guid>.SuccessAsync(propertyType.Id, _localizer["Property Type Saved"]);
            }
            catch (Exception)
            {
                throw new PropertyTypeCustomException(_localizer, null);
            }
        }

        public async Task<Result<Guid>> Handle(UpdatePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            if (!await _context.PropertyTypes
                .Where(x => x.Id == command.Id)
                .AnyAsync(x => x.Name == command.Name, cancellationToken))
            {
                throw new PropertyTypeNotFoundException(_localizer);
            }

            var propertyType = _mapper.Map<PropertyType>(command);
            if (command.FileUploadRequest != null)
            {
                var fileUploadRequest = new FileUploadRequest
                {
                    Data = command.FileUploadRequest?.Data,
                    Extension = Path.GetExtension(command.FileUploadRequest.FileName),
                    UploadStorageType = UploadStorageType.PropertyType
                };
                string fileName = await _propertyTypeService.GenerateFileName(10);
                fileUploadRequest.FileName = $"{fileName}.{fileUploadRequest.Extension}";
                propertyType.ImageUrl = await _uploadService.UploadAsync(fileUploadRequest, FileType.Image);
            }

            propertyType.IsActive = command.IsActive || propertyType.IsActive;
            try
            {
                propertyType.AddDomainEvent(new PropertyTypeUpdatedEvent(propertyType));
                _context.PropertyTypes.Update(propertyType);
                await _context.SaveChangesAsync(cancellationToken);
                await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, PropertyType>(command.Id), cancellationToken);
                return await Result<Guid>.SuccessAsync(propertyType.Id, _localizer["Property Type Updated"]);
            }
            catch (Exception)
            {
                throw new PropertyTypeCustomException(_localizer, null);
            }
        }

        public async Task<Result<Guid>> Handle(RemovePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            var propertyType = await _context.PropertyTypes
                .Where(x => x.Id == command.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (propertyType == null)
            {
                throw new PropertyTypeNotFoundException(_localizer);
            }

            try
            {
                propertyType.AddDomainEvent(new PropertyTypeRemovedEvent(propertyType.Id));
                _context.PropertyTypes.Remove(propertyType);
                await _context.SaveChangesAsync(cancellationToken);
                await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, PropertyType>(command.Id), cancellationToken);
                return await Result<Guid>.SuccessAsync(propertyType.Id, _localizer["Property Type Deleted"]);
            }
            catch (Exception)
            {
                throw new PropertyTypeCustomException(_localizer, null);
            }
        }
    }
}