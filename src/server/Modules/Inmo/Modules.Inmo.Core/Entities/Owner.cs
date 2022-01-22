// --------------------------------------------------------------------------------------------------
// <copyright file="Owner.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations.Schema;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Inmo.Core.Entities
{
    public class Owner : BaseEntity
    {
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

        [NotMapped]
        public string FullName => $"{Name} {SurName}";
    }
}