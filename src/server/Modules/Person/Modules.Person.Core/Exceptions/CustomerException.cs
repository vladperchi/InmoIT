// --------------------------------------------------------------------------------------------------
// <copyright file="CustomerException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Net;
using InmoIT.Shared.Core.Exceptions;

namespace InmoIT.Modules.Person.Core.Exceptions
{
    public class CustomerException : CustomException
    {
        public CustomerException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message, statusCode: statusCode)
        {
        }
    }
}