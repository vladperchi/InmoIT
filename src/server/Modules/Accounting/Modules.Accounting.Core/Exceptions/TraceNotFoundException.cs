// --------------------------------------------------------------------------------------------------
// <copyright file="TraceNotFoundException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Net;
using InmoIT.Shared.Core.Exceptions;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Accounting.Core.Exceptions
{
    public class TraceNotFoundException : CustomException
    {
        public Guid Id { get; }

        public TraceNotFoundException(IStringLocalizer localizer, Guid id)
            : base(localizer[$"Trace with Id: {id} was not found."], null, HttpStatusCode.NotFound)
        {
            Id = id;
        }
    }
}