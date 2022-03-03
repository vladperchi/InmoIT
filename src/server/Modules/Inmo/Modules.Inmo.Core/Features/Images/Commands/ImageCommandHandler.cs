// --------------------------------------------------------------------------------------------------
// <copyright file="ImageCommandHandler.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InmoIT.Modules.Inmo.Core.Abstractions;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Exceptions;
using InmoIT.Modules.Inmo.Core.Features.Images.Events;
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

namespace InmoIT.Modules.Inmo.Core.Features.Images.Commands
{
    public class ImageCommandHandler :
        IRequestHandler<AddImageCommand, Result<List<Guid>>>,
        IRequestHandler<UpdateImageCommand, Result<Guid>>,
        IRequestHandler<RemoveImageCommand, Result<Guid>>
    {
        private readonly IDistributedCache _cache;
        private readonly IInmoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ImageCommandHandler> _localizer;
        private readonly IPropertyImageService _propertyImageService;
        private readonly IUploadService _uploadService;

        public ImageCommandHandler(
            IDistributedCache cache,
            IInmoDbContext context,
            IMapper mapper,
            IStringLocalizer<ImageCommandHandler> localizer,
            IPropertyImageService propertyImageService,
            IUploadService uploadService)
        {
            _cache = cache;
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _propertyImageService = propertyImageService;
            _uploadService = uploadService;
        }

        public async Task<Result<List<Guid>>> Handle(AddImageCommand command, CancellationToken cancellationToken)
        {
            if (!await _context.Properties.Where(x => x.Id == command.PropertyId).AnyAsync(cancellationToken))
            {
                throw new PropertyNotFoundException(_localizer, command.PropertyId);
            }

            var result = new Result<List<Guid>>();

            foreach (var item in command.PropertyImageList)
            {
                if (await _context.PropertyImages.Where(x => x.PropertyId == command.PropertyId).AnyAsync(x => x.CodeImage == item.CodeImage, cancellationToken))
                {
                    throw new ImageAlreadyExistsException(_localizer);
                }

                var image = _mapper.Map<PropertyImage>(item);
                image.CodeImage.ToUpper();
                if (!string.IsNullOrWhiteSpace(item.Data) && !string.IsNullOrWhiteSpace(item.FileName))
                {
                    var fileUploadRequest = new FileUploadRequest
                    {
                        Data = item.Data,
                        Extension = Path.GetExtension(item.FileName),
                        UploadStorageType = UploadStorageType.Property
                    };
                    string fileName = await _propertyImageService.GenerateFileName(20);
                    fileUploadRequest.FileName = $"{fileName}.{fileUploadRequest.Extension}";
                    image.ImageUrl = await _uploadService.UploadAsync(fileUploadRequest, FileType.Image);
                }

                try
                {
                    image.AddDomainEvent(new ImageAddedEvent(image));
                    await _context.PropertyImages.AddAsync(image, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    result.Data.Add(image.Id);
                }
                catch (Exception)
                {
                    throw new ImageCustomException(_localizer, null);
                }
            }

            return await Result<List<Guid>>.SuccessAsync(result.Data, _localizer["Added Property Images"]);
        }

        public async Task<Result<Guid>> Handle(UpdateImageCommand command, CancellationToken cancellationToken)
        {
            if (!await _context.PropertyImages.Where(x => x.Id == command.Id).AnyAsync(x => x.CodeImage == command.CodeImage, cancellationToken))
            {
                throw new ImageNotFoundException(_localizer, command.Id);
            }

            var image = _mapper.Map<PropertyImage>(command);
            string currentImageUrl = command.ImageUrl ?? string.Empty;
            if (command.DeleteCurrentImageUrl && !string.IsNullOrEmpty(currentImageUrl))
            {
                await _uploadService.RemoveFileImage(UploadStorageType.Property, currentImageUrl);
                image = image.ClearPathImageUrl();
                var fileUploadRequest = new FileUploadRequest
                {
                    Data = command.Data,
                    Extension = Path.GetExtension(command.FileName),
                    UploadStorageType = UploadStorageType.Property
                };
                string fileName = await _propertyImageService.GenerateFileName(20);
                fileUploadRequest.FileName = $"{fileName}.{fileUploadRequest.Extension}";
                image.ImageUrl = await _uploadService.UploadAsync(fileUploadRequest, FileType.Image);
}

            image.CodeImage = command.CodeImage.ToUpper() ?? image.CodeImage;
            try
            {
                image.AddDomainEvent(new ImageUpdatedEvent(image));
                _context.PropertyImages.Update(image);
                await _context.SaveChangesAsync(cancellationToken);
                await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, PropertyImage>(command.Id), cancellationToken);
            }
            catch (Exception)
            {
                throw new ImageCustomException(_localizer, null);
            }

            return await Result<Guid>.SuccessAsync(image.Id, _localizer["Updated Property Image"]);
        }

        public async Task<Result<Guid>> Handle(RemoveImageCommand command, CancellationToken cancellationToken)
        {
            var image = await _context.PropertyImages.Where(x => x.Id == command.Id).FirstOrDefaultAsync(cancellationToken);
            _ = image ?? throw new ImageNotFoundException(_localizer, command.Id);
            try
            {
                image.AddDomainEvent(new ImageRemovedEvent(image.Id));
                _context.PropertyImages.RemoveRange(image);
                await _context.SaveChangesAsync(cancellationToken);
                await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, PropertyImage>(command.Id), cancellationToken);
                return await Result<Guid>.SuccessAsync(image.Id, _localizer["Deleted Property Image"]);
            }
            catch (Exception)
            {
                throw new ImageCustomException(_localizer, null);
            }
        }
    }
}