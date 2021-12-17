// --------------------------------------------------------------------------------------------------
// <copyright file="UserLoggedEvent.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Identity.Core.Events
{
    public class UserLoggedEvent : Event
    {
        public Guid UserId { get; }

        public new DateTime Timestamp { get; }

        public UserLoggedEvent(Guid userId)
        {
            UserId = userId;
            Timestamp = DateTime.Now;
            AggregateId = userId;
            RelatedEntities = new[] { typeof(InmoUser) };
        }
    }
}