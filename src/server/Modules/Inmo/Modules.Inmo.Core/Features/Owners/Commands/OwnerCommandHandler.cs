// --------------------------------------------------------------------------------------------------
// <copyright file="OwnerCommandHandler.cs" company="InmoIT">
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
using InmoIT.Modules.Inmo.Core.Features.Owners.Events;
using InmoIT.Shared.Core.Common;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Wrapper;
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
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<OwnerCommandHandler> _localizer;

        public OwnerCommandHandler(
            IInmoDbContext context,
            IMapper mapper,
            IUploadService uploadService,
            IStringLocalizer<OwnerCommandHandler> localizer,
            IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _cache = cache;
        }

        public async Task<Result<Guid>> Handle(RegisterOwnerCommand command, CancellationToken cancellationToken)
        {
            var owner = await _context.Owners.Where(c => c.Name == command.Name && c.SurName == command.SurName && c.Address == command.Address && c.Email == command.Email && c.PhoneNumber == command.PhoneNumber && c.Birthday == command.Birthday)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
            if (owner != null)
            {
                throw new OwnerAlreadyExistsException(_localizer);
            }

            owner = _mapper.Map<Owner>(command);
            var fileUploadRequest = command.FileUploadRequest;
            if (fileUploadRequest != null)
            {
                fileUploadRequest.FileName = $"O-{command.FileFullName}.{fileUploadRequest.Extension}";
                owner.ImageUrl = await _uploadService.UploadAsync(fileUploadRequest, FileType.Image);
            }

            owner.Gender = owner.Gender.NullToString() ?? GenderConstant.GenderType.Male;
            owner.Group = owner.Group.NullToString() ?? GroupConstant.GroupType.Normal;
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
            var customer = await _context.Owners.Where(c => c.Id == command.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
            if (customer == null)
            {
                throw new OwnerNotFoundException(_localizer);
            }

            customer = _mapper.Map<Owner>(command);
            var fileUploadRequest = command.FileUploadRequest;
            if (fileUploadRequest != null)
            {
                fileUploadRequest.FileName = $"C-{command.FileFullName}{fileUploadRequest.Extension}";
                customer.ImageUrl = await _uploadService.UploadAsync(fileUploadRequest, FileType.Image);
            }

            customer.Gender = customer.Gender.NullToString() ?? GenderConstant.GenderType.Male;
            customer.Group = customer.Group.NullToString() ?? GroupConstant.GroupType.Normal;
            try
            {
                customer.AddDomainEvent(new OwnerUpdatedEvent(customer));
                _context.Owners.Update(customer);
                await _context.SaveChangesAsync(cancellationToken);
                await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, Owner>(command.Id), cancellationToken);
                return await Result<Guid>.SuccessAsync(customer.Id, _localizer["Customer Updated"]);
            }
            catch (Exception)
            {
                throw new OwnerCustomException(_localizer, null);
            }
        }

        public async Task<Result<Guid>> Handle(RemoveOwnerCommand command, CancellationToken cancellationToken)
        {
            var customer = await _context.Owners.FirstOrDefaultAsync(b => b.Id == command.Id, cancellationToken);
            if (customer == null)
            {
                throw new OwnerNotFoundException(_localizer);
            }

            try
            {
                customer.AddDomainEvent(new OwnerRemovedEvent(customer.Id));
                _context.Owners.Remove(customer);
                await _context.SaveChangesAsync(cancellationToken);
                await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, Owner>(command.Id), cancellationToken);
                return await Result<Guid>.SuccessAsync(customer.Id, _localizer["Owner Deleted"]);
            }
            catch (Exception)
            {
                throw new OwnerCustomException(_localizer, null);
            }
        }
    }
}