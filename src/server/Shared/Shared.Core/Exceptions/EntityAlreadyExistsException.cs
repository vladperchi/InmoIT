// --------------------------------------------------------------------------------------------------
// <copyright file="EntityAlreadyExistsException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Net;
using Microsoft.Extensions.Localization;

namespace InmoIT.Shared.Core.Exceptions
{
    public class EntityAlreadyExistsException : CustomException
    {
        public EntityAlreadyExistsException(IStringLocalizer localizer)
            : base(localizer["The entity already exists..."], null, HttpStatusCode.BadRequest)
        {
        }
    }
}