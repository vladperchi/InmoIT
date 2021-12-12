// --------------------------------------------------------------------------------------------------
// <copyright file="IdentityCustomException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Localization;

namespace InmoIT.Shared.Core.Exceptions
{
    public class IdentityCustomException : CustomException
    {
        public IdentityCustomException(IStringLocalizer localizer, List<string> errors)
            : base(localizer["Identity errors have occurred..."], errors, HttpStatusCode.InternalServerError)
        {
        }
    }
}