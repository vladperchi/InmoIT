// --------------------------------------------------------------------------------------------------
// <copyright file="ImageEventHandler.cs" company="InmoIT">
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

namespace InmoIT.Modules.Inmo.Core.Features.Images.Events
{
    public class ImageEventHandler :
        INotificationHandler<ImageAddedEvent>,
        INotificationHandler<ImageUpdatedEvent>,
        INotificationHandler<ImageRemovedEvent>
    {
        private readonly ILogger<ImageEventHandler> _logger;
        private readonly IStringLocalizer<ImageEventHandler> _localizer;

        public ImageEventHandler(
            ILogger<ImageEventHandler> logger,
            IStringLocalizer<ImageEventHandler> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public Task Handle(ImageAddedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(ImageAddedEvent)} High. Added {notification.Id}."]);
            return Task.CompletedTask;
        }

        public Task Handle(ImageUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(ImageUpdatedEvent)} High. Updated {notification.Id}."]);
            return Task.CompletedTask;
        }

        public Task Handle(ImageRemovedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(ImageRemovedEvent)} High. Removed {notification.Id}."]);
            return Task.CompletedTask;
        }
    }
}