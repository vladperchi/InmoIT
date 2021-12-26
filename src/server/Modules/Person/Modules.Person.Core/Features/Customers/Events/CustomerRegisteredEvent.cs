// --------------------------------------------------------------------------------------------------
// <copyright file="CustomerRegisteredEvent.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Person.Core.Features.Customers.Events
{
    public class CustomerRegisteredEvent : Event
    {
        public Guid Id { get; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public string Group { get; set; }

        public string Email { get; }

        public string ImageUrl { get; }

        public CustomerRegisteredEvent(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            SurName = customer.SurName;
            PhoneNumber = customer.PhoneNumber;
            Gender = customer.Gender;
            Group = customer.Group;
            Email = customer.Email;
            ImageUrl = customer.ImageUrl;
            AggregateId = customer.Id;
            RelatedEntities = new[] { typeof(Customer) };
            EventDescription = $"Registered Customer Id {customer.Id}.";
        }
    }
}