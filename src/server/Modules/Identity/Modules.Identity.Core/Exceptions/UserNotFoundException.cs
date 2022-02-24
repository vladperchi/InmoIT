// --------------------------------------------------------------------------------------------------
// <copyright file="UserNotFoundException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Net;
using InmoIT.Shared.Core.Exceptions;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Identity.Core.Exceptions
{
    public class UserNotFoundException : CustomException
    {
        public string User { get; }

        public UserNotFoundException(IStringLocalizer localizer, string user)
            : base(localizer[$"User: {user} was not found..."], null, HttpStatusCode.NotFound)
        {
            User = user;
        }
    }
}