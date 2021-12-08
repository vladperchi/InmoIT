// --------------------------------------------------------------------------------------------------
// <copyright file="IdentityException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Net;

namespace InmoIT.Shared.Core.Exceptions
{
    public class IdentityException : CustomException
    {
        public IdentityException(string message, List<string> errors = default, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message, errors, statusCode)
        {
        }
    }
}