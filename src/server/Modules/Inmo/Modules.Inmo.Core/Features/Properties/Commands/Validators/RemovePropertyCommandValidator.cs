// --------------------------------------------------------------------------------------------------
// <copyright file="RemovePropertyCommandValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.Properties.Commands.Validators
{
    public class RemovePropertyCommandValidator : AbstractValidator<RemovePropertyCommand>
    {
        public RemovePropertyCommandValidator(IStringLocalizer<RemovePropertyCommandValidator> localizer)
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage(_ => localizer["{PropertyName} must not be empty."]);
        }
    }
}