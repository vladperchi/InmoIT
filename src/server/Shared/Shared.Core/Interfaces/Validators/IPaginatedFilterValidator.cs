// --------------------------------------------------------------------------------------------------
// <copyright file="IPaginatedFilterValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Core.Contracts;
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Dtos.Filters;
using FluentValidation;

using Microsoft.Extensions.Localization;

namespace InmoIT.Shared.Core.Interfaces.Validators
{
    internal interface IPaginatedFilterValidator<TEntityId, TEntity, TFilter>
        where TEntity : class, IEntity<TEntityId>
        where TFilter : PaginatedFilter
    {
        static void UseRules(AbstractValidator<TFilter> validator, IStringLocalizer localizer)
        {
            validator.RuleFor(request => request.PageNumber)
                .GreaterThan(0).WithMessage(localizer["The {PropertyName} property must be greater than 0."]);
            validator.RuleFor(request => request.PageSize)
                .GreaterThan(0).WithMessage(localizer["The {PropertyName} property must be greater than 0."]);
            validator.RuleFor(request => request.OrderBy)
                .MustContainCorrectOrderingsFor(typeof(TEntity), localizer);
        }
    }
}