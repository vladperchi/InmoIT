// --------------------------------------------------------------------------------------------------
// <copyright file="RemoveCartCommandValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Features.Carts.Commands.Validators
{
    public class RemoveCartCommandValidator : AbstractValidator<RemoveCartCommand>
    {
        public RemoveCartCommandValidator(
            IStringLocalizer<RemoveCartCommandValidator> localizer)
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage(_ => localizer["{PropertyName} must not be empty."]);
        }
    }
}