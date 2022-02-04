// --------------------------------------------------------------------------------------------------
// <copyright file="RemovePropertyTypeCommandValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Commands.Validators
{
    public class RemovePropertyTypeCommandValidator : AbstractValidator<RemovePropertyTypeCommand>
    {
        public RemovePropertyTypeCommandValidator(IStringLocalizer<RemovePropertyTypeCommandValidator> localizer)
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage(_ => localizer["The {PropertyName} property cannot be empty."]);
        }
    }
}