// --------------------------------------------------------------------------------------------------
// <copyright file="PaginatedCartItemFilterValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Shared.Core.Features.Queries.Validators;
using InmoIT.Shared.Dtos.Person.CartItems;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Features.CartItems.Queries.Validators
{
    public class PaginatedCartItemFilterValidator : PaginatedFilterValidator<Guid, CartItem, PaginatedCartItemFilter>
    {
        /// <summary>
        /// You can override the validation rules here.
        /// </summary>
        public PaginatedCartItemFilterValidator(IStringLocalizer<PaginatedCartItemFilterValidator> localizer)
            : base(localizer)
        {
        }
    }
}