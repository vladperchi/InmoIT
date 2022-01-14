// --------------------------------------------------------------------------------------------------
// <copyright file="AddCartItemCommandValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using FluentValidation;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Features.CartItems.Commands.Validators
{
    public class AddCartItemCommandValidator : AbstractValidator<AddCartItemCommand>
    {
        public AddCartItemCommandValidator(IStringLocalizer<AddCartItemCommandValidator> localizer)
        {
            RuleFor(x => x.CartId)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."]);
            RuleFor(x => x.PropertyId)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."]);
        }
    }
}