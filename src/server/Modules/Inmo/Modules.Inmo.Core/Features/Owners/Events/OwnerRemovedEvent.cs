// --------------------------------------------------------------------------------------------------
// <copyright file="OwnerRemovedEvent.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Inmo.Core.Features.Owners.Events
{
    public class OwnerRemovedEvent : Event
    {
        public Guid Id { get; }

        public OwnerRemovedEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
            RelatedEntities = new[] { typeof(Owner) };
            EventDescription = $"Deleted Owner:{id}";
        }
    }
}