// --------------------------------------------------------------------------------------------------
// <copyright file="PaginatedLogFilterValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Logging;
using InmoIT.Shared.Dtos.Identity.Logging;
using InmoIT.Shared.Core.Features.Queries.Validators;
using Microsoft.Extensions.Localization;

namespace InmoIT.Shared.Infrastructure.Validators
{
    public class PaginatedLogFilterValidator : PaginatedFilterValidator<Guid, EventLog, PaginatedLogFilter>
    {
        public PaginatedLogFilterValidator(IStringLocalizer<PaginatedLogFilterValidator> localizer)
            : base(localizer)
        {
            // Here you can override the validation rules
        }
    }
}