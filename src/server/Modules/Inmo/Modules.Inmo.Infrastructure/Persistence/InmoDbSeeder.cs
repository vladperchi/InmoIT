// --------------------------------------------------------------------------------------------------
// <copyright file="InmoDbSeeder.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Infrastructure.Persistence.Resources;
using InmoIT.Shared.Core.Interfaces.Serialization.Serializer;
using InmoIT.Shared.Core.Interfaces.Services;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace InmoIT.Modules.Inmo.Infrastructure.Persistence
{
    public class InmoDbSeeder : IDbSeeder
    {
        private readonly ILogger<InmoDbSeeder> _logger;
        private readonly InmoDbContext _context;
        private readonly IStringLocalizer<InmoDbSeeder> _localizer;
        private readonly IJsonSerializer _jsonSerializer;

        public InmoDbSeeder(
            ILogger<InmoDbSeeder> logger,
            InmoDbContext context,
            IStringLocalizer<InmoDbSeeder> localizer,
            IJsonSerializer jsonSerializer)
        {
            _logger = logger;
            _context = context;
            _localizer = localizer;
            _jsonSerializer = jsonSerializer;
        }

        public void Initialize()
        {
            try
            {
                AddOwners();
                AddPropertyTypes();
                AddProperties();
                _context.SaveChanges();
            }
            catch (Exception)
            {
                _logger.LogError(_localizer["An error occurred while seeding data module Inmo."]);
            }
        }

        private void AddOwners()
        {
            Task.Run(async () =>
            {
                // string fileName = Path.Combine("Files", SeedsConstant.Owner.OwnersSeed);
                // string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);

                if (!_context.Owners.Any())
                {
                    var dataDeserialize = DeserializeJson<Owner>(Seeds.Owners);
                    if (dataDeserialize != null)
                    {
                        foreach (var item in dataDeserialize)
                        {
                            await _context.Owners.AddAsync(item);
                        }

                        await _context.SaveChangesAsync();
                        _logger.LogInformation(_localizer["Seeded Owners Successfully."]);
                    }
                }
            }).GetAwaiter().GetResult();
        }

        private void AddPropertyTypes()
        {
            Task.Run(async () =>
            {
                if (!_context.PropertyTypes.Any())
                {
                    var dataDeserialize = DeserializeJson<PropertyType>(Seeds.PropertyTypes);
                    if (dataDeserialize != null)
                    {
                        foreach (var item in dataDeserialize)
                        {
                            await _context.PropertyTypes.AddAsync(item);
                        }

                        await _context.SaveChangesAsync();
                        _logger.LogInformation(_localizer["Seeded Property Types Successfully."]);
                    }
                }
            }).GetAwaiter().GetResult();
        }

        private void AddPropertyImages()
        {
            Task.Run(async () =>
            {
                if (!_context.PropertyImages.Any())
                {
                    var dataDeserialize = DeserializeJson<PropertyImage>(Seeds.PropertyImages);
                    if (dataDeserialize != null)
                    {
                        foreach (var item in dataDeserialize)
                        {
                            await _context.PropertyImages.AddAsync(item);
                        }

                        await _context.SaveChangesAsync();
                        _logger.LogInformation(_localizer["Seeded Property Images Successfully."]);
                    }
                }
            }).GetAwaiter().GetResult();
        }

        private void AddProperties()
        {
            Task.Run(async () =>
            {
                if (!_context.Properties.Any())
                {
                    var dataDeserialize = DeserializeJson<Property>(Seeds.Properties);
                    if (dataDeserialize != null)
                    {
                        foreach (var item in dataDeserialize)
                        {
                            await _context.Properties.AddAsync(item);
                        }

                        await _context.SaveChangesAsync();
                        _logger.LogInformation(_localizer["Seeded Properties Successfully."]);
                    }
                }
            }).GetAwaiter().GetResult();
        }

        private List<T> DeserializeJson<T>(byte[] data)
            => _jsonSerializer.Deserialize<List<T>>(Encoding.Default.GetString(data));
    }
}