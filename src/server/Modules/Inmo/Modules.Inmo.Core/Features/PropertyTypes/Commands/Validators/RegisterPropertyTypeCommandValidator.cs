// --------------------------------------------------------------------------------------------------
// <copyright file="RegisterPropertyTypeCommandValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Linq;
using System.Text.RegularExpressions;

using FluentValidation;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Commands.Validators
{
    public class RegisterPropertyTypeCommandValidator : AbstractValidator<CreatePropertyTypeCommand>
    {
        public RegisterPropertyTypeCommandValidator(
            IStringLocalizer<RegisterPropertyTypeCommandValidator> localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Length(10, 100).WithMessage(localizer["{PropertyName} must have between 10 and 100 characters."])
                .NotEqual(x => x.Description).WithMessage(localizer["{PropertyName} cannot be equal to description."])
                .NotEqual(x => x.CodeInternal).WithMessage(localizer["{PropertyName} cannot be equal to code internal."])
                .Must(IsOnlyLetterAndSpace).WithMessage(localizer["{PropertyName} should be all letters."]);
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Length(20, 150).WithMessage(localizer["{PropertyName} must have between 20 and 150 characters."])
                .NotEqual(x => x.Name).WithMessage(localizer["{PropertyName} cannot be equal to Name."])
                .NotEqual(x => x.CodeInternal).WithMessage(localizer["{PropertyName} cannot be equal to code internal."])
                .Must(IsOnlyLetterAndSpace).WithMessage(localizer["{PropertyName} should be all letters."]);
            RuleFor(x => x.CodeInternal)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .MaximumLength(3).WithMessage(localizer["{PropertyName} must have maximu 3 characters."])
                .NotEqual(x => x.Name).WithMessage(localizer["{PropertyName} cannot be equal to Name."])
                .NotEqual(x => x.Description).WithMessage(localizer["{PropertyName} cannot be equal to description."])
                .Must(IsLetterOrDigit).WithMessage(localizer["{PropertyName} must be only letters and numbers."]);
        }

        public bool IsOnlyLetterAndSpace(string value) => Regex.IsMatch(value, @"^(?! )[A-Za-z\s]+$");

        private bool IsLetterOrDigit(string value) => value.All(char.IsLetterOrDigit);
    }
}