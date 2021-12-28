// --------------------------------------------------------------------------------------------------
// <copyright file="Owner.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Inmo.Core.Entities
{
    public class Owner : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string ImageUrl { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Birthday { get; set; }
    }
}