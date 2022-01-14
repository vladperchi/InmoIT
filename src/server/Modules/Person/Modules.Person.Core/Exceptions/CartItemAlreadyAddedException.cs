// --------------------------------------------------------------------------------------------------
// <copyright file="CartItemAlreadyAddedException.cs" company="InmoIT">
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
    public class CartItemAlreadyAddedException : CustomException
    {
        public CartItemAlreadyAddedException(IStringLocalizer localizer)
            : base(localizer["Cart Item already added to the Cart..."], null, HttpStatusCode.BadRequest)
        {
        }
    }
}