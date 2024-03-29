﻿// --------------------------------------------------------------------------------------------------
// <copyright file="EventLogListEmptyException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Net;
using Microsoft.Extensions.Localization;

namespace InmoIT.Shared.Core.Exceptions
{
    public class EventLogListEmptyException : CustomException
    {
        public EventLogListEmptyException(IStringLocalizer localizer)
            : base(localizer["Event log List is empty..."], null, HttpStatusCode.NoContent)
        {
        }
    }
}