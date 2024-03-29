﻿// --------------------------------------------------------------------------------------------------
// <copyright file="SmsTwilioService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Dtos.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace InmoIT.Shared.Infrastructure.Services
{
    /// <inheritdoc cref = "ISmsTwilioService" />
    public class SmsTwilioService : ISmsTwilioService
    {
        private readonly SmsTwilioSettings _smsTwilioSettings;
        private readonly ILogger<SmsTwilioService> _logger;

        public SmsTwilioService(IOptions<SmsTwilioSettings> smsTwilioSettings, ILogger<SmsTwilioService> logger)
        {
            _smsTwilioSettings = smsTwilioSettings.Value;
            _logger = logger;
}

        public Task SendAsync(SmsTwilioRequest request)
        {
            try
            {
                string accountSid = _smsTwilioSettings.Identification;
                string authToken = _smsTwilioSettings.Password;

                TwilioClient.Init(accountSid, authToken);
                return MessageResource.CreateAsync(
                    to: new PhoneNumber(request.Number),
                    from: new PhoneNumber(_smsTwilioSettings.From),
                    body: request.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"An error occurred while send SMS to number {request.Number}");
            }

            _logger.LogInformation($"Send SMS to number {request.Number} successfull");
            return Task.CompletedTask;
        }
    }
}