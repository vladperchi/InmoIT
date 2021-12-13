// --------------------------------------------------------------------------------------------------
// <copyright file="MessageService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

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
    public class MessageService : IMessageService
    {
        private readonly MessageSettings _settings;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IOptions<MessageSettings> settings, ILogger<MessageService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
}

        public Task SendAsync(MessageRequest request)
        {
            try
            {
                string accountSid = _settings.MessageIdentification;
                string authToken = _settings.MessagePassword;

                TwilioClient.Init(accountSid, authToken);

                return MessageResource.CreateAsync(
                    to: new PhoneNumber(request.Number),
                    from: new PhoneNumber(_settings.MessageFrom),
                    body: request.Message);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return Task.CompletedTask;
        }
    }
}