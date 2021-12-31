// --------------------------------------------------------------------------------------------------
// <copyright file="OwnerRegisteredEvent.cs" company="InmoIT">
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
    public class OwnerRegisteredEvent : Event
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public string Address { get; set; }

        public string ImageUrl { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Birthday { get; set; }

        public OwnerRegisteredEvent(Owner owner)
        {
            Id = owner.Id;
            Name = owner.Name;
            SurName = owner.SurName;
            Address = owner.Address;
            ImageUrl = owner.ImageUrl;
            Email = owner.Email;
            PhoneNumber = owner.PhoneNumber;
            Birthday = owner.Birthday;
            AggregateId = owner.Id;
            RelatedEntities = new[] { typeof(Owner) };
            EventDescription = $"Owner {Name} {SurName} registered.";
        }
    }
}