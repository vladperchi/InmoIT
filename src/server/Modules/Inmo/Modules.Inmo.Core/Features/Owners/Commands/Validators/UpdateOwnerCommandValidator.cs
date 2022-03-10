// --------------------------------------------------------------------------------------------------
// <copyright file="UpdateOwnerCommandValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Text.RegularExpressions;

using FluentValidation;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.Owners.Commands.Validators
{
    public class UpdateOwnerCommandValidator : AbstractValidator<UpdateOwnerCommand>
    {
        public UpdateOwnerCommandValidator(
            IStringLocalizer<UpdateOwnerCommandValidator> localizer)
        {
            RuleFor(x => x.Id)
                  .NotEqual(Guid.Empty).WithMessage(_ => localizer["{PropertyName} must not be empty."]);
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Length(10, 100).WithMessage(localizer["{PropertyName} must have between 10 and 100 characters."])
                .NotEqual(x => x.SurName).WithMessage(localizer["{PropertyName} cannot be equal to Surname."])
                .NotEqual(x => x.Address).WithMessage(localizer["{PropertyName} cannot be equal to Address."])
                .Must(IsOnlyLetterAndSpace).WithMessage(localizer["{PropertyName} should be all letters."]);
            RuleFor(x => x.SurName)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Length(10, 100).WithMessage(localizer["{PropertyName} must have between 10 and 100 characters."])
                .NotEqual(x => x.Name).WithMessage(localizer["{PropertyName} cannot be equal to Name."])
                .NotEqual(x => x.Address).WithMessage(localizer["{PropertyName} cannot be equal to Address."])
                .Must(IsOnlyLetterAndSpace).WithMessage(localizer["{PropertyName} should be all letters."]);
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Length(20, 150).WithMessage(localizer["{PropertyName} must have between 20 and 150 characters."])
                .NotEqual(x => x.Name).WithMessage(localizer["{PropertyName} cannot be equal to Name."])
                .NotEqual(x => x.SurName).WithMessage(localizer["{PropertyName} cannot be equal to Surname."]);
            RuleFor(x => x.Email)
               .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
               .EmailAddress().WithMessage(localizer["{PropertyName} must be a valid email accounts."]);
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Length(8, 16).WithMessage(localizer["{PropertyName} must have between 8 and  16 characters."])
                .Must(IsOnlyNumber).WithMessage(localizer["{PropertyName} must be only numbers."]);
            RuleFor(x => x.Birthday)
                .NotNull().WithMessage(localizer["{PropertyName} must not be empty."])
                .LessThan(DateTime.MaxValue).WithMessage(localizer["{PropertyName} should be less than max value"])
                .Must(YouOverMore18).WithMessage(localizer["{PropertyName} you must be of legal age to register."]);
        }

        public bool IsOnlyLetterAndSpace(string value) => Regex.IsMatch(value, @"^(?! )[A-Za-z\s]+$");

        private bool IsOnlyNumber(string value) => value.All(char.IsNumber);

        private bool YouOverMore18(DateTime value) => DateTime.Now.AddYears(-18) >= value;
    }
}