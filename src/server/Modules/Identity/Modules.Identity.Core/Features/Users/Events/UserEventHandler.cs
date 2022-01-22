// --------------------------------------------------------------------------------------------------
// <copyright file="UserEventHandler.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace InmoIT.Modules.Identity.Core.Features.Users.Events
{
    public class UserEventHandler :
        INotificationHandler<UserRegisteredEvent>,
        INotificationHandler<UserUpdatedEvent>,
        INotificationHandler<UserDeletedEvent>
    {
        private readonly ILogger<UserEventHandler> _logger;
        private readonly IStringLocalizer<UserEventHandler> _localizer;

        public UserEventHandler(
            ILogger<UserEventHandler> logger,
            IStringLocalizer<UserEventHandler> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(UserRegisteredEvent)} High. Registered {notification.Id}."]);
            return Task.CompletedTask;
        }

        public Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(UserUpdatedEvent)} High. Updated {notification.Id}."]);
            return Task.CompletedTask;
        }

        public Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(UserDeletedEvent)} High. Deleted {notification.Id}."]);
            return Task.CompletedTask;
        }
    }
}