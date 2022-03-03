// --------------------------------------------------------------------------------------------------
// <copyright file="CustomerCommandHandler.cs" company="InmoIT">
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
using InmoIT.Modules.Person.Core.Abstractions;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Modules.Person.Core.Exceptions;
using InmoIT.Modules.Person.Core.Features.Customers.Events;
using InmoIT.Shared.Core.Common.Enums;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Integration.Person;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Upload;
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
        private readonly ICustomerService _customerService;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<CustomerCommandHandler> _localizer;

        public CustomerCommandHandler(
            ICustomerDbContext context,
            IMapper mapper,
            ICustomerService customerService,
            IUploadService uploadService,
            IStringLocalizer<CustomerCommandHandler> localizer,
            IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _customerService = customerService;
            _uploadService = uploadService;
            _localizer = localizer;
            _cache = cache;
        }

        public async Task<Result<Guid>> Handle(RegisterCustomerCommand command, CancellationToken cancellationToken)
        {
            if (await _context.Customers.Where(x => x.Email == command.Email).AnyAsync(x => x.PhoneNumber == command.PhoneNumber, cancellationToken))
            {
                throw new CustomerAlreadyExistsException(_localizer);
            }

            var customer = _mapper.Map<Customer>(command);
            if (command.FileUploadRequest != null)
            {
                var fileUploadRequest = new FileUploadRequest
                {
                    Data = command.FileUploadRequest?.Data,
                    Extension = Path.GetExtension(command.FileUploadRequest.FileName),
                    UploadStorageType = UploadStorageType.Customer
                };
                string fileName = await _customerService.GenerateFileName(20);
                fileUploadRequest.FileName = $"{fileName}.{fileUploadRequest.Extension}";
                customer.ImageUrl = await _uploadService.UploadAsync(fileUploadRequest, FileType.Image);
            }

            customer.IsActive = true;
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
            if (!await _context.Customers.Where(x => x.Id == command.Id).AnyAsync(cancellationToken))
            {
                throw new CustomerNotFoundException(_localizer, command.Id);
            }

            var customer = _mapper.Map<Customer>(command);
            string currentImageUrl = command.ImageUrl ?? string.Empty;
            if (command.DeleteCurrentImageUrl && !string.IsNullOrEmpty(currentImageUrl))
            {
                await _uploadService.RemoveFileImage(UploadStorageType.Customer, currentImageUrl);
                customer = customer.ClearPathImageUrl();
                var fileUploadRequest = new FileUploadRequest
                {
                    Data = command.FileUploadRequest?.Data,
                    Extension = Path.GetExtension(command.FileUploadRequest.FileName),
                    UploadStorageType = UploadStorageType.Customer
                };
                string fileName = await _customerService.GenerateFileName(20);
                fileUploadRequest.FileName = $"{fileName}.{fileUploadRequest.Extension}";
                customer.ImageUrl = await _uploadService.UploadAsync(fileUploadRequest, FileType.Image);
            }

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
            var customer = await _context.Customers.Where(x => x.Id == command.Id).FirstOrDefaultAsync(cancellationToken);
            _ = customer ?? throw new CustomerNotFoundException(_localizer, command.Id);
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