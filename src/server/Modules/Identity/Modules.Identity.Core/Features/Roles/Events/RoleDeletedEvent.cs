// --------------------------------------------------------------------------------------------------
// <copyright file="RoleDeletedEvent.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Identity.Core.Features.Roles.Events
{
    public class RoleDeletedEvent : Event
    {
        public string Id { get; }

        public RoleDeletedEvent(string id)
        {
            Id = id;
            AggregateId = Guid.TryParse(id, out var aggregateId) ? aggregateId : Guid.NewGuid();
            RelatedEntities = new[] { typeof(InmoRole) };
            EventDescription = "Deleted Role.";
        }
    }
}