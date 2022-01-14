// --------------------------------------------------------------------------------------------------
// <copyright file="PaginatedCartFilterValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Shared.Core.Features.Queries.Validators;
using InmoIT.Shared.Dtos.Person.Carts;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Features.Carts.Queries.Validators
{
    public class PaginatedCartFilterValidator : PaginatedFilterValidator<Guid, Cart, PaginatedCartFilter>
    {
        /// <summary>
        /// You can override the validation rules here.
        /// </summary>
        public PaginatedCartFilterValidator(IStringLocalizer<PaginatedCartFilterValidator> localizer)
            : base(localizer)
        {
        }
    }
}