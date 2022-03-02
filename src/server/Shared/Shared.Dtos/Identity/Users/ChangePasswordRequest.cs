// --------------------------------------------------------------------------------------------------
// <copyright file="ChangePasswordRequest.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Dtos.Identity.Users
{
    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
    }
}