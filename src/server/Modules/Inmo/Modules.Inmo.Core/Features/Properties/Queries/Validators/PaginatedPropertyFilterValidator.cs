// --------------------------------------------------------------------------------------------------
// <copyright file="PaginatedPropertyFilterValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Features.Queries.Validators;
using InmoIT.Shared.Dtos.Inmo.Properties;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.Properties.Queries.Validators
{
    public class PaginatedPropertyFilterValidator : PaginatedFilterValidator<Guid, Property, PaginatedPropertyFilter>
    {
        /// <summary>
        /// You can override the validation rules here.
        /// </summary>
        public PaginatedPropertyFilterValidator(IStringLocalizer<PaginatedPropertyFilterValidator> localizer)
            : base(localizer)
        {
        }
    }
}