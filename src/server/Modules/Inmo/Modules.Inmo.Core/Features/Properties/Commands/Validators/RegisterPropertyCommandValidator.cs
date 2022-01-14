// --------------------------------------------------------------------------------------------------
// <copyright file="RegisterPropertyCommandValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Linq;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.Properties.Commands.Validators
{
    public class RegisterPropertyCommandValidator : AbstractValidator<RegisterPropertyCommand>
    {
        public RegisterPropertyCommandValidator(
            IStringLocalizer<RegisterPropertyCommandValidator> localizer)
        {
            RuleFor(c => c.Name)
               .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
               .Length(20, 150).WithMessage(localizer["{PropertyName} property must have between 20 and 150 characters."]);
            RuleFor(c => c.Address)
               .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
               .Length(20, 150).WithMessage(localizer["{PropertyName} must have between 20 and 150 characters."]);
            RuleFor(c => c.Description)
               .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
               .Length(50, 250).WithMessage(localizer["{PropertyName} must have between 50 and 250 characters."]);
            RuleFor(c => c.OwnerId)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."]);
            RuleFor(c => c.Price)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Must(IsOnlyDigit).WithMessage(localizer["{PropertyName} should be all decimal."])
                .GreaterThan(0);
            RuleFor(c => c.Tax)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Must(IsOnlyDigit).WithMessage(localizer["{PropertyName} should be all decimal."]);
            RuleFor(c => c.Year)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Must(IsOnlyNumber).WithMessage(localizer["{PropertyName} should be all numbers."]);
        }

        private bool IsOnlyDigit(decimal value) => value.ToString().All(char.IsDigit);

        private bool IsOnlyNumber(int value) => value.ToString().All(char.IsNumber);
    }
}