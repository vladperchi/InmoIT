// --------------------------------------------------------------------------------------------------
// <copyright file="UserListEmptyException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Net;
using InmoIT.Shared.Core.Exceptions;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Identity.Core.Exceptions
{
    public class UserListEmptyException : CustomException
    {
        public UserListEmptyException(IStringLocalizer localizer)
            : base(localizer["User List is empty..."], null, HttpStatusCode.NoContent)
        {
        }
    }
}