// --------------------------------------------------------------------------------------------------
// <copyright file="Customer.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Person.Core.Entities
{
    public class Customer : BaseEntity
    {
        public Customer()
        {
        }

        public string Name { get; set; }

        public string SurName { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public string Group { get; set; }

        public string Email { get; set; }

        public string ImageUrl { get; set; }
    }
}