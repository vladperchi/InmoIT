// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyTypeEventHandler.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using InmoIT.Modules.Inmo.Core.Features.Owners.Events;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Events
{
    public class PropertyTypeEventHandler :
        INotificationHandler<PropertyTypeRegisteredEvent>,
        INotificationHandler<PropertyTypeUpdatedEvent>,
        INotificationHandler<PropertyTypeRemovedEvent>
    {
        private readonly ILogger<PropertyTypeEventHandler> _logger;
        private readonly IStringLocalizer<PropertyTypeEventHandler> _localizer;

        public PropertyTypeEventHandler(
            ILogger<PropertyTypeEventHandler> logger,
            IStringLocalizer<PropertyTypeEventHandler> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public Task Handle(PropertyTypeRegisteredEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(PropertyTypeRegisteredEvent)} High. {notification.Id} Registered."]);
            return Task.CompletedTask;
        }

        public Task Handle(PropertyTypeUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(PropertyTypeUpdatedEvent)} High. {notification.Id} Updated."]);
            return Task.CompletedTask;
        }

        public Task Handle(PropertyTypeRemovedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(PropertyTypeRemovedEvent)} High. {notification.Id} Removed."]);
            return Task.CompletedTask;
        }
    }
}