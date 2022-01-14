// --------------------------------------------------------------------------------------------------
// <copyright file="CartItemCustomException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Localization;
using InmoIT.Shared.Core.Exceptions;

namespace InmoIT.Modules.Person.Core.Exceptions
{
    public class CartItemCustomException : CustomException
    {
        public CartItemCustomException(IStringLocalizer localizer, List<string> errors)
            : base(localizer["Cart Item errors have occurred..."], errors, HttpStatusCode.InternalServerError)
        {
        }
    }
}