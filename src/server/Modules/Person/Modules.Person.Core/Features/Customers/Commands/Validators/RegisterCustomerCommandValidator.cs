// --------------------------------------------------------------------------------------------------
// <copyright file="RegisterCustomerCommandValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using FluentValidation;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Features.Customers.Commands.Validators
{
    public class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
    {
        public RegisterCustomerCommandValidator(IStringLocalizer<RegisterCustomerCommandValidator> localizer)
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage(localizer["{PropertyName} cannot be empty."])
                .Length(8, 150).WithMessage(localizer["{PropertyName} must have between 8 and 150 characters."]);
            RuleFor(c => c.SurName)
                .NotEmpty().WithMessage(localizer["{PropertyName} cannot be empty."])
                .Length(8, 150).WithMessage(localizer["{PropertyName} must have between 8 and 150 characters."]);
            RuleFor(c => c.PhoneNumber)
                .NotEmpty().WithMessage(localizer["{PropertyName} cannot be empty."])
                .Length(8, 30).WithMessage(localizer["{PropertyName} must have between 8 and 30 characters."]);
            RuleFor(c => c.Gender)
                .NotEmpty().WithMessage(localizer["{PropertyName} cannot be empty."]);
            RuleFor(c => c.Group)
                .NotEmpty().WithMessage(localizer["{PropertyName} cannot be empty."]);
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage(localizer["{PropertyName} cannot be empty."]);
        }
    }
}