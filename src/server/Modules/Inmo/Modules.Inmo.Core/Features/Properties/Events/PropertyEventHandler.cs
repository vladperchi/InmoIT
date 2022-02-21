// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyEventHandler.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Threading;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace InmoIT.Modules.Inmo.Core.Features.Properties.Events
{
    public class PropertyEventHandler :
        INotificationHandler<PropertyRegisteredEvent>,
        INotificationHandler<PropertyUpdatedEvent>,
        INotificationHandler<PropertyRemovedEvent>
    {
        private readonly ILogger<PropertyEventHandler> _logger;
        private readonly IStringLocalizer<PropertyEventHandler> _localizer;

        public PropertyEventHandler(
            ILogger<PropertyEventHandler> logger,
            IStringLocalizer<PropertyEventHandler> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public Task Handle(PropertyRegisteredEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(PropertyRegisteredEvent)} High. {notification.EventDescription}"]);
            return Task.CompletedTask;
        }

        public Task Handle(PropertyUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(PropertyUpdatedEvent)} High. {notification.EventDescription}"]);
            return Task.CompletedTask;
        }

        public Task Handle(PropertyRemovedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(PropertyRemovedEvent)} High. {notification.EventDescription}"]);
            return Task.CompletedTask;
        }
    }
}