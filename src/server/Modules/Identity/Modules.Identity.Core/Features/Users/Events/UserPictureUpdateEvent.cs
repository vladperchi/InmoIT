// --------------------------------------------------------------------------------------------------
// <copyright file="UserPictureUpdateEvent.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Identity.Core.Features.Users.Events
{
    public class UserPictureUpdateEvent : Event
    {
        public string Id { get; }

        public string ImageUrl { get; }

        public UserPictureUpdateEvent(InmoUser user)
        {
            Id = user.Id;
            ImageUrl = user.ImageUrl;
            AggregateId = Guid.TryParse(user.Id, out var aggregateId)
                ? aggregateId
                : Guid.NewGuid();
            RelatedEntities = new[] { typeof(InmoUser) };
            EventDescription = "Updated User Picture.";
        }
    }
}