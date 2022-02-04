// --------------------------------------------------------------------------------------------------
// <copyright file="RegisterPropertyTypeCommandValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Linq;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Commands.Validators
{
    public class RegisterPropertyTypeCommandValidator : AbstractValidator<RegisterPropertyTypeCommand>
    {
        public RegisterPropertyTypeCommandValidator(
            IStringLocalizer<RegisterPropertyTypeCommandValidator> localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Length(10, 100).WithMessage(localizer["{PropertyName} must have between 10 and 100 characters."])
                .NotEqual(x => x.Description).WithMessage(localizer["{PropertyName} cannot be equal to Description."])
                .Must(IsOnlyLetter).WithMessage(localizer["{PropertyName} should be all letters."]);
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Length(20, 150).WithMessage(localizer["{PropertyName} must have between 20 and 150 characters."])
                .NotEqual(x => x.Name).WithMessage(localizer["{PropertyName} cannot be equal to Name."]);
        }

        private bool IsOnlyLetter(string value) => value.All(char.IsLetter);
    }
}