// --------------------------------------------------------------------------------------------------
// <copyright file="PaginatedFilterValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using FluentValidation;
using InmoIT.Shared.Core.Contracts;
using InmoIT.Shared.Core.Interfaces.Validators;
using InmoIT.Shared.Dtos.Filters;
using Microsoft.Extensions.Localization;

namespace InmoIT.Shared.Core.Features.Queries.Validators
{
    public abstract class PaginatedFilterValidator<TEntityId, TEntity, TFilter> :
        AbstractValidator<TFilter>,
        IPaginatedFilterValidator<TEntityId, TEntity, TFilter>
            where TEntity : class, IEntity<TEntityId>
            where TFilter : PaginatedFilter
    {
        protected PaginatedFilterValidator(IStringLocalizer localizer)
        {
            IPaginatedFilterValidator<TEntityId, TEntity, TFilter>.UseRules(this, localizer);
        }
    }
}