﻿// --------------------------------------------------------------------------------------------------
// <copyright file="OwnerRegisteredEvent.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Domain;

using static InmoIT.Shared.Core.Constants.PermissionsConstant;

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

        public DateTime Birthday { get; set; }

        public string Gender { get; set; }

        public string Group { get; set; }

        public bool IsActive { get; set; }

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
            Gender = owner.Gender;
            Group = owner.Group;
            IsActive = owner.IsActive;
            AggregateId = owner.Id;
            RelatedEntities = new[] { typeof(Owner) };
            EventDescription = $"Registered Owner:{owner.Name} {owner.SurName}:::Email:{owner.Email}:::Id:{owner.Id}";
}
    }
}