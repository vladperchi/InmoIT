// --------------------------------------------------------------------------------------------------
// <copyright file="RoleClaimDeletedEvent.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Identity.Core.Features.RoleClaims.Events
{
    public class RoleClaimDeletedEvent : Event
    {
        public int Id { get; }

        public RoleClaimDeletedEvent(int id)
{
            Id = id;
            AggregateId = Guid.TryParse(id.ToString(), out var aggregateId) ? aggregateId : Guid.NewGuid();
            RelatedEntities = new[] { typeof(InmoRoleClaim) };
            EventDescription = "Deleted RoleClaim.";
        }
    }
}