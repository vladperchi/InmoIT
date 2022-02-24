// --------------------------------------------------------------------------------------------------
// <copyright file="DocumentNotFoundException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Net;
using InmoIT.Shared.Core.Exceptions;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Document.Core.Exceptions
{
    public class DocumentNotFoundException : CustomException
    {
        public Guid Id { get; }

        public DocumentNotFoundException(IStringLocalizer localizer, Guid id)
            : base(localizer[$"Document with Id: {id} was not found."], null, HttpStatusCode.NotFound)
        {
            Id = id;
        }
    }
}