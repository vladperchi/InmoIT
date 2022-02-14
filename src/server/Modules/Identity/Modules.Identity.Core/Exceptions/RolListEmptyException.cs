// --------------------------------------------------------------------------------------------------
// <copyright file="RolListEmptyException.cs" company="InmoIT">
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
    public class RolListEmptyException : CustomException
    {
        public RolListEmptyException(IStringLocalizer localizer)
            : base(localizer["Rol list is empty..."], null, HttpStatusCode.NoContent)
        {
        }
    }
}