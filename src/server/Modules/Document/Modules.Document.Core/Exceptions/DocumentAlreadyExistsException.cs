// --------------------------------------------------------------------------------------------------
// <copyright file="DocumentAlreadyExistsException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Net;
using InmoIT.Shared.Core.Exceptions;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Document.Core.Exceptions
{
    public class DocumentAlreadyExistsException : CustomException
    {
        public DocumentAlreadyExistsException(IStringLocalizer localizer)
            : base(localizer["Document already exists..."], null, HttpStatusCode.BadRequest)
        {
        }
    }
}