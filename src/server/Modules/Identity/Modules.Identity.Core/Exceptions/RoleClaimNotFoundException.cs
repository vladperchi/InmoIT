// --------------------------------------------------------------------------------------------------
// <copyright file="RoleClaimNotFoundException.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Net;
using InmoIT.Shared.Core.Exceptions;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Identity.Core.Exceptions
{
    public class RoleClaimNotFoundException : CustomException
    {
        public int Id { get; }

        public RoleClaimNotFoundException(IStringLocalizer localizer, int id)
            : base(localizer[$"Role Claim with Id: {id} was not found."], null, HttpStatusCode.NotFound)
        {
            Id = id;
        }
    }
}