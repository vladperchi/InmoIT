// --------------------------------------------------------------------------------------------------
// <copyright file="UserRegisteredEvent.cs" company="InmoIT">
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
    public class UserRegisteredEvent : Event
    {
        public string Id { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }

        public string UserName { get; }

        public string PhoneNumber { get; }

        public UserRegisteredEvent(InmoUser user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            UserName = user.UserName;
            PhoneNumber = user.PhoneNumber;
            AggregateId = Guid.TryParse(user.Id, out var aggregateId) ? aggregateId : Guid.NewGuid();
            RelatedEntities = new[] { typeof(InmoUser) };
            EventDescription = "Registered User.";
        }
    }
}