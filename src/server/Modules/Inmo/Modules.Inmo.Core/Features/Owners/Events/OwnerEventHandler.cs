// --------------------------------------------------------------------------------------------------
// <copyright file="OwnerEventHandler.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;
using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.Owners.Events
{
    public class OwnerEventHandler :
        INotificationHandler<OwnerRegisteredEvent>,
        INotificationHandler<OwnerUpdatedEvent>,
        INotificationHandler<OwnerRemovedEvent>
    {
        private readonly ILogger<OwnerEventHandler> _logger;
        private readonly IStringLocalizer<OwnerEventHandler> _localizer;

        public OwnerEventHandler(
            ILogger<OwnerEventHandler> logger,
            IStringLocalizer<OwnerEventHandler> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public Task Handle(OwnerRegisteredEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(OwnerRegisteredEvent)} High."]);
            return Task.CompletedTask;
        }

        public Task Handle(OwnerUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(OwnerUpdatedEvent)} High."]);
            return Task.CompletedTask;
        }

        public Task Handle(OwnerRemovedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(OwnerRemovedEvent)} High. {notification.Id} Removed."]);
            return Task.CompletedTask;
        }
    }
}