// --------------------------------------------------------------------------------------------------
// <copyright file="CustomerNotFoundException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Net;
using InmoIT.Shared.Core.Exceptions;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Exceptions
{
    public class CustomerNotFoundException : CustomException
    {
        public CustomerNotFoundException(IStringLocalizer localizer)
            : base(localizer["Customer was not found..."], null, HttpStatusCode.NotFound)
        {
        }
    }
}