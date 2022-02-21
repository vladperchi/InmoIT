// --------------------------------------------------------------------------------------------------
// <copyright file="CustomerNotFoundException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Net;
using InmoIT.Shared.Core.Exceptions;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Exceptions
{
    public class CustomerNotFoundException : CustomException
    {
        public Guid Id { get; }

        public CustomerNotFoundException(IStringLocalizer localizer, Guid id)
            : base(localizer[$"Customer with Id: {id} was not found."], null, HttpStatusCode.NotFound)
        {
            Id = id;
        }
    }
}