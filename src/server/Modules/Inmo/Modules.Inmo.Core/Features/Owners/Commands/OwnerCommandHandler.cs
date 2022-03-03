// --------------------------------------------------------------------------------------------------
// <copyright file="OwnerCommandHandler.cs" company="InmoIT">
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

namespace InmoIT.Modules.Inmo.Core.Features.Owners.Commands
{
    internal class OwnerCommandHandler :
        IRequestHandler<RegisterOwnerCommand, Result<Guid>>,
        IRequestHandler<UpdateOwnerCommand, Result<Guid>>,
        IRequestHandler<RemoveOwnerCommand, Result<Guid>>
    {
        private readonly IDistributedCache _cache;
        private readonly IInmoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IOwnerService _ownerService;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<OwnerCommandHandler> _localizer;

        public OwnerCommandHandler(
            IInmoDbContext context,
            IMapper mapper,
            IOwnerService ownerService,
            IUploadService uploadService,
            IStringLocalizer<OwnerCommandHandler> localizer,
            IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _ownerService = ownerService;
            _uploadService = uploadService;
            _localizer = localizer;
            _cache = cache;
        }

        public async Task<Result<Guid>> Handle(RegisterOwnerCommand command, CancellationToken cancellationToken)
{
            if (await _context.Owners.Where(x => x.Email == command.Email).AnyAsync(x => x.PhoneNumber == command.PhoneNumber, cancellationToken))
            {
                throw new OwnerAlreadyExistsException(_localizer);
            }

            var owner = _mapper.Map<Owner>(command);
            if (command.FileUploadRequest != null)
            {
                var fileUploadRequest = new FileUploadRequest
                {
                    Data = command.FileUploadRequest?.Data,
                    Extension = Path.GetExtension(command.FileName),
                    UploadStorageType = UploadStorageType.Owner
                };
                string fileName = await _ownerService.GenerateFileName(20);
                fileUploadRequest.FileName = $"{fileName}.{fileUploadRequest.Extension}";
                owner.ImageUrl = await _uploadService.UploadAsync(fileUploadRequest, FileType.Image);
            }

            owner.IsActive = true;
            owner.Gender ??= GendersConstant.GenderType.Male;
            owner.Group ??= GroupsConstant.GroupType.Normal;
            try
            {
                owner.AddDomainEvent(new OwnerRegisteredEvent(owner));
                await _context.Owners.AddAsync(owner, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<Guid>.SuccessAsync(owner.Id, _localizer["Owner Saved"]);
            }
            catch (Exception)
            {
                throw new OwnerCustomException(_localizer, null);
            }
        }

        public async Task<Result<Guid>> Handle(UpdateOwnerCommand command, CancellationToken cancellationToken)
        {
            if (!await _context.Owners.Where(x => x.Id == command.Id).AnyAsync(x => x.PhoneNumber == command.PhoneNumber, cancellationToken))
            {
                throw new OwnerNotFoundException(_localizer, command.Id);
            }

            var owner = _mapper.Map<Owner>(command);
            string currentImageUrl = command.ImageUrl ?? string.Empty;
            if (command.DeleteCurrentImageUrl && !string.IsNullOrEmpty(currentImageUrl))
            {
                await _uploadService.RemoveFileImage(UploadStorageType.Owner, currentImageUrl);
                owner = owner.ClearPathImageUrl();
                var fileUploadRequest = new FileUploadRequest
                {
                    Data = command.FileUploadRequest?.Data,
                    Extension = Path.GetExtension(command.FileUploadRequest.FileName),
                    UploadStorageType = UploadStorageType.Owner
                };
                string fileName = await _ownerService.GenerateFileName(20);
                fileUploadRequest.FileName = $"{fileName}.{fileUploadRequest.Extension}";
                owner.ImageUrl = await _uploadService.UploadAsync(fileUploadRequest, FileType.Image);
            }

            owner.IsActive = command.IsActive || owner.IsActive;
            try
            {
                owner.AddDomainEvent(new OwnerUpdatedEvent(owner));
                _context.Owners.Update(owner);
                await _context.SaveChangesAsync(cancellationToken);
                await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, Owner>(command.Id), cancellationToken);
                return await Result<Guid>.SuccessAsync(owner.Id, _localizer["Customer Updated"]);
            }
            catch (Exception)
            {
                throw new OwnerCustomException(_localizer, null);
            }
        }

        public async Task<Result<Guid>> Handle(RemoveOwnerCommand command, CancellationToken cancellationToken)
        {
            var owner = await _context.Owners.Where(x => x.Id == command.Id).FirstOrDefaultAsync(cancellationToken);
            _ = owner ?? throw new OwnerNotFoundException(_localizer, command.Id);
            try
            {
                owner.AddDomainEvent(new OwnerRemovedEvent(owner.Id));
                _context.Owners.Remove(owner);
                await _context.SaveChangesAsync(cancellationToken);
                await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, Owner>(command.Id), cancellationToken);
                return await Result<Guid>.SuccessAsync(owner.Id, _localizer["Owner Deleted"]);
            }
            catch (Exception)
            {
                throw new OwnerCustomException(_localizer, null);
            }
        }
    }
}