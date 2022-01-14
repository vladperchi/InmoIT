// --------------------------------------------------------------------------------------------------
// <copyright file="CartEventHandler.cs" company="InmoIT">
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

namespace InmoIT.Modules.Person.Core.Features.Carts.Events
{
    public class CartEventHandler :
        INotificationHandler<CartCreatedEvent>,
        INotificationHandler<CartRemovedEvent>
    {
        private readonly ILogger<CartEventHandler> _logger;
        private readonly IStringLocalizer<CartEventHandler> _localizer;

        public CartEventHandler(ILogger<CartEventHandler> logger, IStringLocalizer<CartEventHandler> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public Task Handle(CartCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(CartCreatedEvent)} High."]);
            return Task.CompletedTask;
        }

        public Task Handle(CartRemovedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[$"{nameof(CartRemovedEvent)} High. {notification.Id} Removed."]);
            return Task.CompletedTask;
        }
    }
}