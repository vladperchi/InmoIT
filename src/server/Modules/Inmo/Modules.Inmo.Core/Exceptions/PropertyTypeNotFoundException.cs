﻿// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyTypeNotFoundException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Net;
using InmoIT.Shared.Core.Exceptions;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Exceptions
{
    public class PropertyTypeNotFoundException : CustomException
    {
        public Guid Id { get; }

        public PropertyTypeNotFoundException(IStringLocalizer localizer)
            : base(localizer["Property Type was not found..."], null, HttpStatusCode.NotFound)
        {
        }
    }
}