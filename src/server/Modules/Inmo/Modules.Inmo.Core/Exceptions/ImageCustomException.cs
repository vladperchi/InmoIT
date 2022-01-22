// --------------------------------------------------------------------------------------------------
// <copyright file="ImageCustomException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Net;
using InmoIT.Shared.Core.Exceptions;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Exceptions
{
    public class ImageCustomException : CustomException
    {
        public ImageCustomException(IStringLocalizer localizer, List<string> errors)
            : base(localizer["Image to property errors have occurred..."], errors, HttpStatusCode.InternalServerError)
        {
        }
    }
}