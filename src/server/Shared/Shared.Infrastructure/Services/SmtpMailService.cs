// --------------------------------------------------------------------------------------------------
// <copyright file="SmtpMailService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Dtos.Mails;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace InmoIT.Shared.Infrastructure.Services
{
    /// <inheritdoc cref = "IMailService" />
    public class SmtpMailService : IMailService
{
        private readonly MailSettings _settings;
        private readonly IStringLocalizer<SmtpMailService> _localizer;
        private readonly ILogger<SmtpMailService> _logger;

        public SmtpMailService(
            IOptions<MailSettings> settings,
            IStringLocalizer<SmtpMailService> localizer,
            ILogger<SmtpMailService> logger)
        {
            _settings = settings.Value;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task SendAsync(MailRequest request)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(_settings.DisplayName, request.From ?? _settings.From));
                foreach (string address in request.To)
                    email.To.Add(MailboxAddress.Parse(address));

                if (!string.IsNullOrEmpty(request.ReplyTo))
                    email.ReplyTo.Add(new MailboxAddress(request.ReplyToName, request.ReplyTo));

                if (request.Bcc != null)
                {
                    foreach (string address in request.Bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
                        email.Bcc.Add(MailboxAddress.Parse(address.Trim()));
                }

                if (request.Cc != null)
                {
                    foreach (string address in request.Cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                        email.Cc.Add(MailboxAddress.Parse(address.Trim()));
                }

                if (request.Headers != null)
                {
                    foreach (var header in request.Headers)
                        email.Headers.Add(header.Key, header.Value);
                }

                var builder = new BodyBuilder();
                email.Sender = new MailboxAddress(request.DisplayName ?? _settings.DisplayName, request.From ?? _settings.From);
                email.Subject = request.Subject;
                builder.HtmlBody = request.Body;

                if (request.AttachmentData != null)
                {
                    foreach (var attachmentInfo in request.AttachmentData)
                        builder.Attachments.Add(attachmentInfo.Key, attachmentInfo.Value);
                }

                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_settings.UserName, _settings.Password);
                await smtp.SendAsync(email);
                _logger.LogInformation(string.Format(_localizer["Send email to {0} successfull"], email.To));
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(_localizer["An error occurred while send email to {0}. Error message: {1}"], request.To, ex.Message));
            }
        }
    }
}