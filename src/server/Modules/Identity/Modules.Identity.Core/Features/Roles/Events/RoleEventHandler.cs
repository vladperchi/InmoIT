// --------------------------------------------------------------------------------------------------
// <copyright file="RoleEventHandler.cs" company="InmoIT">
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

namespace InmoIT.Modules.Identity.Core.Features.Roles.Events
{
    public class RoleEventHandler :
        INotificationHandler<RoleAddedEvent>,
        INotificationHandler<RoleUpdatedEvent>,
        INotificationHandler<RoleDeletedEvent>
    {
        private readonly ILogger<RoleEventHandler> _logger;
        private readonly IStringLocalizer<RoleEventHandler> _localizer;

        public RoleEventHandler(
            ILogger<RoleEventHandler> logger,
            IStringLocalizer<RoleEventHandler> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public Task Handle(RoleAddedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(RoleAddedEvent)} High."]);
            return Task.CompletedTask;
        }

        public Task Handle(RoleUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(RoleUpdatedEvent)} High."]);
            return Task.CompletedTask;
        }

        public Task Handle(RoleDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(RoleDeletedEvent)} High. {notification.Id} Deleted."]);
            return Task.CompletedTask;
        }
    }
}