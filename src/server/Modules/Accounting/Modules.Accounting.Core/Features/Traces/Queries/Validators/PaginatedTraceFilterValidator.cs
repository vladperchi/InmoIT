// --------------------------------------------------------------------------------------------------
// <copyright file="PaginatedTraceFilterValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Accounting.Core.Entities;
using InmoIT.Shared.Core.Features.Queries.Validators;
using InmoIT.Shared.Dtos.Inmo.Traces;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Accounting.Core.Features.Traces.Queries.Validators
{
    public class PaginatedTraceFilterValidator : PaginatedFilterValidator<Guid, PropertyTrace, PaginatedTraceFilter>
    {
        /// <summary>
        /// You can override the validation rules here.
        /// </summary>
        public PaginatedTraceFilterValidator(IStringLocalizer<PaginatedTraceFilterValidator> localizer)
            : base(localizer)
        {
        }
    }
}