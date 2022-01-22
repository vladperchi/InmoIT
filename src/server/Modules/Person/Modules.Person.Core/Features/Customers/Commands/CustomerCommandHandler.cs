// --------------------------------------------------------------------------------------------------
// <copyright file="CustomerCommandHandler.cs" company="InmoIT">
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
using InmoIT.Modules.Person.Core.Abstractions;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Modules.Person.Core.Exceptions;
using InmoIT.Modules.Person.Core.Features.Customers.Events;
using InmoIT.Shared.Core.Common.Extensions;
using InmoIT.Shared.Core.Common.Enums;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Features.Customers.Commands
{
    internal class CustomerCommandHandler :
        IRequestHandler<RegisterCustomerCommand, Result<Guid>>,
        IRequestHandler<RemoveCustomerCommand, Result<Guid>>,
        IRequestHandler<UpdateCustomerCommand, Result<Guid>>
    {
        private readonly IDistributedCache _cache;
        private readonly ICustomerDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<CustomerCommandHandler> _localizer;

        public CustomerCommandHandler(
            ICustomerDbContext context,
            IMapper mapper,
            IUploadService uploadService,
            IStringLocalizer<CustomerCommandHandler> localizer,
            IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _cache = cache;
        }

        public async Task<Result<Guid>> Handle(RegisterCustomerCommand command, CancellationToken cancellationToken)
        {
            if (await _context.Customers
                .Where(x => x.Email == command.Email)
                .AnyAsync(x => x.PhoneNumber == command.PhoneNumber, cancellationToken))
            {
                throw new CustomerAlreadyExistsException(_localizer);
            }

            var customer = _mapper.Map<Customer>(command);
            var fileUploadRequest = command.FileUploadRequest;
            if (fileUploadRequest != null)
            {
                fileUploadRequest.FileName = $"C-{command.FileName}.{fileUploadRequest.Extension}";
                customer.ImageUrl = await _uploadService.UploadAsync(fileUploadRequest, FileType.Image);
            }

            customer.Gender ??= GendersConstant.GenderType.Male;
            customer.Group ??= GroupsConstant.GroupType.Normal;
            try
            {
                customer.AddDomainEvent(new CustomerRegisteredEvent(customer));
                await _context.Customers.AddAsync(customer, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return await Result<Guid>.SuccessAsync(customer.Id, _localizer["Customer Saved"]);
            }
            catch (Exception)
            {
                throw new CustomerCustomException(_localizer, null);
            }
        }

        public async Task<Result<Guid>> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            if (!await _context.Customers
                .Where(x => x.Id == command.Id)
                .AnyAsync(cancellationToken))
            {
                throw new CustomerNotFoundException(_localizer);
            }

            var customer = _mapper.Map<Customer>(command);
            var fileUploadRequest = command.FileUploadRequest;
            if (fileUploadRequest != null)
            {
                fileUploadRequest.FileName = $"C-{command.FileName}{fileUploadRequest.Extension}";
                customer.ImageUrl = await _uploadService.UploadAsync(fileUploadRequest, FileType.Image);
            }

            customer.Name = command.Name ?? customer.Name;
            customer.SurName = command.SurName ?? customer.SurName;
            customer.Email = command.Email ?? customer.Email;
            customer.PhoneNumber = command.PhoneNumber ?? customer.PhoneNumber;
            customer.Gender = command.Gender ?? customer.Gender;
            customer.Group = command.Group ?? customer.Group;
            customer.IsActive = command.IsActive || customer.IsActive;

            try
            {
                customer.AddDomainEvent(new CustomerUpdatedEvent(customer));
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync(cancellationToken);
                await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, Customer>(command.Id), cancellationToken);
                return await Result<Guid>.SuccessAsync(customer.Id, _localizer["Customer Updated"]);
            }
            catch (Exception)
            {
                throw new CustomerCustomException(_localizer, null);
            }
        }

        public async Task<Result<Guid>> Handle(RemoveCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers
                 .Where(x => x.Id == command.Id)
                 .FirstOrDefaultAsync(cancellationToken);
            if (customer == null)
            {
                throw new CustomerNotFoundException(_localizer);
            }

            try
            {
                customer.AddDomainEvent(new CustomerRemovedEvent(customer.Id));
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync(cancellationToken);
                await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<Guid, Customer>(command.Id), cancellationToken);
                return await Result<Guid>.SuccessAsync(customer.Id, _localizer["Customer Deleted"]);
            }
            catch (Exception)
            {
                throw new CustomerCustomException(_localizer, null);
            }
        }
    }
}