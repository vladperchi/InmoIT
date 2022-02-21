// --------------------------------------------------------------------------------------------------
// <copyright file="UpdateUserRequest.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace InmoIT.Shared.Dtos.Identity.Users
{
    public class UpdateUserRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [MinLength(8)]
        public string CurrentPassword { get; set; }

        [MinLength(8)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}