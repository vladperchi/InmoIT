// --------------------------------------------------------------------------------------------------
// <copyright file="EventLog.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Contracts;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Shared.Core.Entities
{
    public class EventLog : Event, IEntity<Guid>
    {
        public EventLog(Event eventLog, string data, (string oldValues, string newValues) changes, string email, Guid userId)
        {
            Id = Guid.NewGuid();
            AggregateId = eventLog.AggregateId;
            MessageType = eventLog.MessageType;
            Data = data;
            Email = email;
            OldValues = changes.oldValues;
            NewValues = changes.newValues;
            UserId = userId;
            EventDescription = eventLog.EventDescription;
        }

        protected EventLog()
        {
        }

        public Guid Id { get; set; }

        public string Data { get; private set; }

        public string OldValues { get; private set; }

        public string NewValues { get; private set; }

        public string Email { get; private set; }

        public Guid UserId { get; private set; }
    }
}