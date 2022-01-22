// --------------------------------------------------------------------------------------------------
// <copyright file="ImageCommandHandler.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
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
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.Images.Commands
{
    public class ImageCommandHandler :
        IRequestHandler<AddImageCommand, Result<Guid>>,
        IRequestHandler<UpdateImageCommand, Result<Guid>>,
        IRequestHandler<RemoveImageCommand, Result<Guid>>
    {
        private readonly IDistributedCache _cache;
        private readonly IInmoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ImageCommandHandler> _localizer;
        private readonly IUploadService _uploadService;

        public ImageCommandHandler(
            IDistributedCache cache,
            IInmoDbContext context,
            IMapper mapper,
            IStringLocalizer<ImageCommandHandler> localizer,
            IUploadService uploadService)
        {
            _cache = cache;
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _uploadService = uploadService;
        }

        public async Task<Result<Guid>> Handle(AddImageCommand command, CancellationToken cancellationToken)
        {
            if (!await _context.Properties
                .Where(x => x.Id == command.PropertyId)
                .AnyAsync(cancellationToken))
            {
                throw new PropertyNotFoundException(_localizer);
            }

            var data = await _context.PropertyImages
                .Where(x => x.PropertyId == command.PropertyId)
                .ToListAsync(cancellationToken);

            if (data.Count == 0)
            {
                throw new ImageListEmptyException(_localizer);
            }

            if (await _context.PropertyImages
                .Where(x => x.PropertyId == command.PropertyId)
                .AnyAsync(x => x.CodeImage == command.CodeImage, cancellationToken))
            {
                throw new ImageAlreadyExistsException(_localizer);
            }

            var result = new Result<Guid>();
            var image = _mapper.Map<PropertyImage>(command);
            var fileUploadRequest = command.FileUploadRequest;
            try
            {
                if (fileUploadRequest != null && command.ImageData.Count > 0)
                {
                    foreach (string item in command.ImageData)
                    {
                        fileUploadRequest.Data = item[0].ToString();
                        fileUploadRequest.FileName = $"P-{command.CodeImage}.{fileUploadRequest.Extension}";
                        image.ImageUrl = await _uploadService.UploadAsync(fileUploadRequest, FileType.Image);
                        image.AddDomainEvent(new ImageAddedEvent(image));
                        await _context.PropertyImages.AddAsync(image, cancellationToken);
                        await _context.SaveChangesAsync(cancellationToken);
                        result = await Result<Guid>.SuccessAsync(image.Id, _localizer["Added Image Property"]);
                    }
                }

                return result;
            }
            catch (Exception)
            {
                throw new ImageCustomException(_localizer, null);
            }
        }

        public async Task<Result<Guid>> Handle(UpdateImageCommand command, CancellationToken cancellationToken)
        {
            var data = await _context.PropertyImages
                .Where(x => x.PropertyId == command.PropertyId)
                .ToListAsync(cancellationToken);

            if (data.Count == 0)
            {
                throw new ImageListEmptyException(_localizer);
            }

            if (await _context.PropertyImages
                .AsNoTracking()
                .AnyAsync(x => x.Id != command.Id && x.CodeImage == command.CodeImage && x.PropertyId == command.PropertyId, cancellationToken))
            {
                throw new ImageAlreadyAddedException(_localizer);
            }

            var result = new Result<Guid>();
            var image = _mapper.Map<PropertyImage>(command);
            var fileUploadRequest = command.FileUploadRequest;
            try
            {
                if (fileUploadRequest != null && command.ImageData.Count > 0)
                {
                    foreach (string item in command.ImageData)
                    {
                        fileUploadRequest.Data = item[0].ToString();
                        fileUploadRequest.FileName = $"P-{command.CodeImage}.{fileUploadRequest.Extension}";
                        image.ImageUrl = await _uploadService.UploadAsync(fileUploadRequest, FileType.Image);
                        image.AddDomainEvent(new ImageUpdatedEvent(image));
                        _context.PropertyImages.Update(image);
                        await _context.SaveChangesAsync(cancellationToken);
                        await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, PropertyImage>(command.Id), cancellationToken);
                        result = await Result<Guid>.SuccessAsync(image.Id, _localizer["Updated Image Property"]);
                    }
                }

                return result;
            }
            catch (Exception)
            {
                throw new ImageCustomException(_localizer, null);
            }
        }

        public async Task<Result<Guid>> Handle(RemoveImageCommand command, CancellationToken cancellationToken)
        {
            if (!await _context.Properties
               .Where(x => x.Id == command.Id)
               .AnyAsync(cancellationToken))
            {
                throw new PropertyNotFoundException(_localizer);
            }

            var data = await _context.PropertyImages
                .Where(x => x.PropertyId == command.Id)
                .ToListAsync(cancellationToken);

            if (data.Count == 0)
            {
                throw new ImageListEmptyException(_localizer);
            }

            var result = new Result<Guid>();
            try
            {
                foreach (var item in data)
                {
                    item.AddDomainEvent(new ImageRemovedEvent(item.Id));
                    _context.PropertyImages.RemoveRange(data);
                    await _context.SaveChangesAsync(cancellationToken);
                    await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, PropertyImage>(command.Id), cancellationToken);
                    result = await Result<Guid>.SuccessAsync(item.Id, _localizer["Deleted Image Property"]);
                }

                return result;
            }
            catch (Exception)
            {
                throw new ImageCustomException(_localizer, null);
            }
        }
    }
}