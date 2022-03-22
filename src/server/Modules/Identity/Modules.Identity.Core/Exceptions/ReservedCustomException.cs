// --------------------------------------------------------------------------------------------------
// <copyright file="ReservedCustomException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Net;
using InmoIT.Shared.Core.Exceptions;

using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Identity.Core.Exceptions
{
    public class ReservedCustomException : CustomException
    {
        public ReservedCustomException(IStringLocalizer localizer, List<string> errors)
            : base(localizer["Method reserved for in-scope initialization"], errors, HttpStatusCode.MethodNotAllowed)
        {
        }
    }
}