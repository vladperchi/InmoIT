// --------------------------------------------------------------------------------------------------
// <copyright file="ConnectionDbSure.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Interfaces.Persistence;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;

namespace InmoIT.Shared.Infrastructure.Persistence.Connection
{
    public class ConnectionDbSure : IConnectionDbSure
    {
        private readonly PersistenceSettings _options;
        private readonly IStringLocalizer<ConnectionDbSure> _localizer;
        private readonly ILogger<ConnectionDbSure> _logger;

        public ConnectionDbSure(
            IOptions<PersistenceSettings> options,
            IStringLocalizer<ConnectionDbSure> localizer,
            ILogger<ConnectionDbSure> logger)
        {
            _options = options.Value;
            _localizer = localizer;
            _logger = logger;
        }

        public Task<string> MakeSure(string connectionString, string dataProvider)
        {
            if (string.IsNullOrEmpty(dataProvider))
            {
                switch (true)
                {
                    case true when _options.UseMsSql:
                        dataProvider = DataProviderKeys.SqlServer;
                        if (string.IsNullOrEmpty(_options.ConnectionStrings.MSSQL))
                        {
                            throw new InvalidOperationException("Data Provider is not configured.");
                        }

                        connectionString = _options.ConnectionStrings.MSSQL;
                        break;
                }

                _logger.LogInformation(string.Format(
                    _localizer["Current data provider {0} to establish a secure connection"], dataProvider.ToUpper()));
            }

            try
            {
                _logger.LogInformation(string.Format(
                    _localizer["DB connection sure successfully for Data Provider {0}"], dataProvider.ToUpper()));

                return Task.FromResult(dataProvider switch
                {
                    DataProviderKeys.SqlServer => MakeSureSqlConnectionString(connectionString),
                    _ => connectionString
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(
                    _localizer["Unsecured DB connection for Data Provider {0}. Exception: {1} "], dataProvider.ToUpper(), ex.Message));

                return Task.FromResult("Unsecured DB Connection");
            }
        }

        private string MakeSureSqlConnectionString(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);

            if (!string.IsNullOrEmpty(builder.Password) || !builder.IntegratedSecurity)
            {
                builder.Password = StringKeys.HiddenValue;
            }

            if (!string.IsNullOrEmpty(builder.UserID) || !builder.IntegratedSecurity)
            {
                builder.UserID = StringKeys.HiddenValue;
            }

            return builder.ToString();
        }
    }
}