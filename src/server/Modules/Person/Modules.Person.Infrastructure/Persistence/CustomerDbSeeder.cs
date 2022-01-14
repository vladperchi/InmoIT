// --------------------------------------------------------------------------------------------------
// <copyright file="CustomerDbSeeder.cs" company="InmoIT">
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
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Shared.Core.Constants;
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
                _logger.LogError(_localizer["An error occurred while seeding Customers Data."]);
            }
        }

        private void AddCustomers()
        {
            Task.Run(async () =>
            {
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (!_context.Customers.Any())
                {
                    string customerData = await File.ReadAllTextAsync(path + SeedsConstant.Customer.CustomersData);
                    var customers = _jsonSerializer.Deserialize<List<Customer>>(customerData);

                    if (customers != null)
                    {
                        foreach (var customer in customers)
                        {
                            await _context.Customers.AddAsync(customer);
                        }
                    }

                    await _context.SaveChangesAsync();
                    _logger.LogInformation(_localizer["Seeded Customers."]);
                }
            }).GetAwaiter().GetResult();
        }
    }
}