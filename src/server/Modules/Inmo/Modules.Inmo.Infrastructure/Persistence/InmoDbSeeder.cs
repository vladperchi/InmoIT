// --------------------------------------------------------------------------------------------------
// <copyright file="InmoDbSeeder.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Constants;
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
                AddProperties();
                AddPropertyImages();
                _context.SaveChanges();
            }
            catch (Exception)
            {
                _logger.LogError(_localizer["An error occurred while seeding Inmo Data."]);
            }
        }

        private void AddOwners()
        {
            Task.Run(async () =>
            {
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (!_context.Owners.Any())
                {
                    string dataJSON = await File.ReadAllTextAsync(path + SeedsConstant.Owner.OwnersData);
                    var data = _jsonSerializer.Deserialize<List<Owner>>(dataJSON);

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            await _context.Owners.AddAsync(item);
                        }
                    }

                    await _context.SaveChangesAsync();
                    _logger.LogInformation(_localizer["Seeded Owners Successfully."]);
                }
            }).GetAwaiter().GetResult();
        }

        private void AddProperties()
        {
            Task.Run(async () =>
            {
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (!_context.Properties.Any())
                {
                    string dataJSON = await File.ReadAllTextAsync(path + SeedsConstant.Property.PropertiesData);
                    var data = _jsonSerializer.Deserialize<List<Property>>(dataJSON);

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            await _context.Properties.AddAsync(item);
                        }
                    }

                    await _context.SaveChangesAsync();
                    _logger.LogInformation(_localizer["Seeded Properties Successfully."]);
                }
            }).GetAwaiter().GetResult();
        }

        private void AddPropertyImages()
        {
            Task.Run(async () =>
            {
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (!_context.PropertyImages.Any())
                {
                    string dataJSON = await File.ReadAllTextAsync(path + SeedsConstant.Image.PropertyImagesData);
                    var data = _jsonSerializer.Deserialize<List<PropertyImage>>(dataJSON);

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            await _context.PropertyImages.AddAsync(item);
                        }
                    }

                    await _context.SaveChangesAsync();
                    _logger.LogInformation(_localizer["Seeded Property Images Successfully."]);
                }
            }).GetAwaiter().GetResult();
        }
    }
}