// --------------------------------------------------------------------------------------------------
// <copyright file="OwnerListEmptyException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Net;
using InmoIT.Shared.Core.Exceptions;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Exceptions
{
    public class OwnerListEmptyException : CustomException
    {
        public OwnerListEmptyException(IStringLocalizer localizer)
            : base(localizer["Owner List is empty..."], null, HttpStatusCode.NoContent)
        {
        }
    }
}