// --------------------------------------------------------------------------------------------------
// <copyright file="EventLogCustomException.cs" company="InmoIT">
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
    public class EventLogCustomException : CustomException
    {
        public EventLogCustomException(IStringLocalizer localizer, List<string> errors)
            : base(localizer["Event Log errors have occurred..."], errors, HttpStatusCode.InternalServerError)
        {
        }
    }
}