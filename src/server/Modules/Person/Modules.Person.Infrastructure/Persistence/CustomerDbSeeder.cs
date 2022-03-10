// --------------------------------------------------------------------------------------------------
// <copyright file="CustomerDbSeeder.cs" company="InmoIT">
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
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Modules.Person.Infrastructure.Persistence.Resources;
using InmoIT.Shared.Core.Interfaces.Serialization.Serializer;
using InmoIT.Shared.Core.Interfaces.Services;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace InmoIT.Modules.Person.Infrastructure.Persistence
{
    public class CustomerDbSeeder : IDbSeeder
    {
        private readonly ILogger<CustomerDbSeeder> _logger;
        private readonly CustomerDbContext _context;
        private readonly IStringLocalizer<CustomerDbSeeder> _localizer;
        private readonly IJsonSerializer _jsonSerializer;

        public CustomerDbSeeder(
            ILogger<CustomerDbSeeder> logger,
            CustomerDbContext context,
            IStringLocalizer<CustomerDbSeeder> localizer,
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
                AddCustomers();
                _context.SaveChanges();
            }
            catch (Exception)
            {
                _logger.LogError(_localizer["An error occurred while seeding data module Person."]);
            }
        }

        private void AddCustomers()
        {
            Task.Run(async () =>
            {
                if (!_context.Customers.Any())
                {
                    var dataDeserialize = DeserializeJson<Customer>(Seeds.Customers);
                    if (dataDeserialize != null)
                    {
                        foreach (var customer in dataDeserialize)
                        {
                            await _context.Customers.AddAsync(customer);
                        }

                        await _context.SaveChangesAsync();
                        _logger.LogInformation(_localizer["Seeded Customers Successfully."]);
                    }
                }
            }).GetAwaiter().GetResult();
        }

        private List<T> DeserializeJson<T>(byte[] data) =>
            _jsonSerializer.Deserialize<List<T>>(Encoding.Default.GetString(data));
    }
}