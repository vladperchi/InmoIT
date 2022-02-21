// --------------------------------------------------------------------------------------------------
// <copyright file="UpdateCustomerCommandValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Features.Customers.Commands.Validators
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator(
            IStringLocalizer<UpdateCustomerCommandValidator> localizer)
        {
            RuleFor(x => x.Id)
                  .NotEqual(Guid.Empty).WithMessage(_ => localizer["{PropertyName} must not be empty."]);

            RuleFor(x => x.Name)
                 .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                 .Length(10, 100).WithMessage(localizer["{PropertyName} must have between 10 and 100 characters."])
                 .Equal(x => x.SurName).WithMessage(localizer["{PropertyName} cannot be equal to Surname."])
                 .Must(IsOnlyLetter).WithMessage(localizer["{PropertyName} should be all letters."]);

            RuleFor(x => x.SurName)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Length(10, 100).WithMessage(localizer["{PropertyName} must have between 10 and 100 characters."])
                .Equal(x => x.Name).WithMessage(localizer["{PropertyName} cannot be equal to Name."])
                .Must(IsOnlyLetter).WithMessage(localizer["{PropertyName} should be all letters."]);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Length(8, 16).WithMessage(localizer["{PropertyName} must have between 8 and 30 characters."])
                .Must(IsOnlyNumber).WithMessage(localizer["{PropertyName} should be all numbers."]);

            RuleFor(x => x.Birthday)
                .NotNull().WithMessage(localizer["{PropertyName} must not be empty."])
                .LessThan(DateTime.MaxValue).WithMessage(localizer["{PropertyName} should be less than max value"])
                .Must(YouOverMore18).WithMessage(localizer["{PropertyName} you must be of legal age to register."]);

            RuleFor(x => x.Email)
              .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
              .EmailAddress().WithMessage(localizer["{PropertyName} must be a valid email accounts."]);
        }

        private bool IsOnlyLetter(string value) => value.All(char.IsLetter);

        private bool IsOnlyNumber(string value) => value.All(char.IsNumber);

        private bool YouOverMore18(DateTime value) => DateTime.Now.AddYears(-18) >= value;
    }
}