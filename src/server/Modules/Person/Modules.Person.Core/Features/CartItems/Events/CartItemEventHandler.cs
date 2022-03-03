// --------------------------------------------------------------------------------------------------
// <copyright file="CartItemEventHandler.cs" company="InmoIT">
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

namespace InmoIT.Modules.Person.Core.Features.CartItems.Events
{
    public class CartItemEventHandler :
        INotificationHandler<CartItemAddedEvent>,
        INotificationHandler<CartItemUpdatedEvent>,
        INotificationHandler<CartItemRemovedEvent>
    {
        private readonly ILogger<CartItemEventHandler> _logger;
        private readonly IStringLocalizer<CartItemEventHandler> _localizer;

        public CartItemEventHandler(
            ILogger<CartItemEventHandler> logger,
            IStringLocalizer<CartItemEventHandler> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public Task Handle(CartItemAddedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(CartItemAddedEvent)} High. {notification.EventDescription}"]);
            return Task.CompletedTask;
        }

        public Task Handle(CartItemUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(CartItemUpdatedEvent)} High. {notification.EventDescription}"]);
            return Task.CompletedTask;
        }

        public Task Handle(CartItemRemovedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(CartItemRemovedEvent)} High. {notification.EventDescription}"]);
            return Task.CompletedTask;
        }
    }
}