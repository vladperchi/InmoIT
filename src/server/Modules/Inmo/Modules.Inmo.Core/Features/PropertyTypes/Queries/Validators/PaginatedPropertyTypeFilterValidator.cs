// --------------------------------------------------------------------------------------------------
// <copyright file="PaginatedPropertyTypeFilterValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Features.Queries.Validators;
using InmoIT.Shared.Dtos.Inmo.PropertyTypes;

using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Queries.Validators
{
    public class PaginatedPropertyTypeFilterValidator : PaginatedFilterValidator<Guid, PropertyType, PaginatedPropertyTypeFilter>
    {
        /// <summary>
        /// You can override the validation rules here.
        /// </summary>
        public PaginatedPropertyTypeFilterValidator(IStringLocalizer<PaginatedPropertyTypeFilterValidator> localizer)
            : base(localizer)
        {
        }
    }
}