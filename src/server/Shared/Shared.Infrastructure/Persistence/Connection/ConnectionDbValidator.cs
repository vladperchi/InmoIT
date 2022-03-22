// --------------------------------------------------------------------------------------------------
// <copyright file="ConnectionDbValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Interfaces.Persistence;
using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Core.Wrapper;

using Microsoft.Extensions.Localization;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InmoIT.Shared.Infrastructure.Persistence.Connection
{
    internal class ConnectionDbValidator : IConnectionDbValidator
    {
        private readonly PersistenceSettings _options;
        private readonly IStringLocalizer<ConnectionDbValidator> _localizer;
        private readonly ILogger<ConnectionDbValidator> _logger;

        public ConnectionDbValidator(
            IOptions<PersistenceSettings> options,
            IStringLocalizer<ConnectionDbValidator> localizer,
            ILogger<ConnectionDbValidator> logger)
        {
            _options = options.Value;
            _localizer = localizer;
            _logger = logger;
        }

        public Task<bool> TryValidate(string connectionString, string dataProvider)
        {
            if (string.IsNullOrEmpty(dataProvider))
            {
                switch (true)
                {
                    case true when _options.UseMsSql:
                        dataProvider = DataProviderKeys.SqlServer;
                        break;
                }

                _logger.LogInformation(string.Format(
                    _localizer["Current data provider: {0} try validate connection"], dataProvider.ToUpper()));
            }

            try
            {
                switch (dataProvider)
                {
                    case DataProviderKeys.SqlServer:
                        if (string.IsNullOrEmpty(_options.ConnectionStrings.MSSQL))
                        {
                            throw new InvalidOperationException("Data Provider is not configured.");
                        }

                        connectionString = _options.ConnectionStrings.MSSQL;
                        new SqlConnectionStringBuilder(connectionString);
                        break;
                }

                _logger.LogInformation(string.Format(
                    _localizer["DB connection validation successfully secured for Data Provider {0}"], dataProvider.ToUpper()));

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(
                    _localizer["DB connection validation error for Data Provider {0}. Exception: {1} "], dataProvider.ToUpper(), ex.Message));

                return Task.FromResult(false);
            }
        }
    }
}