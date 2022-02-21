// --------------------------------------------------------------------------------------------------
// <copyright file="RegisterRequest.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace InmoIT.Shared.Dtos.Identity.Users
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(10)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(10)]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Compare("Email")]
        public bool EmailConfirmed { get; set; }

        [Required]
        [MinLength(8)]
        public string UserName { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Compare("PhoneNumber")]
        public bool PhoneNumberConfirmed { get; set; }
    }
}