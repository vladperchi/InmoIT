// --------------------------------------------------------------------------------------------------
// <copyright file="UpdateUserRequestValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;
using InmoIT.Shared.Dtos.Identity.Users;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Identity.Core.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator(IStringLocalizer<UpdateUserRequestValidator> localizer)
        {
            RuleFor(x => x.Id)
                  .NotEqual(string.Empty).WithMessage(_ => localizer["{PropertyName} must not be empty."]);

            RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
            .EmailAddress().WithMessage(localizer["{PropertyName} must be a valid email accounts."]);

            RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Length(8, 16).WithMessage(localizer["{PropertyName} must have between 8 and  16 characters."])
                .Must(IsOnlyNumber).WithMessage(localizer["{PropertyName} should be all numbers."]);

            RuleFor(x => x.FirstName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .MaximumLength(100).WithMessage(localizer["{PropertyName} can have a maximum of 100 characters."])
                .Must(IsOnlyLetterAndSpace).WithMessage(localizer["{PropertyName} should be all letters."])
                .NotEqual(x => x.LastName).WithMessage(localizer["{PropertyName} cannot be equal to LastName."]);

            RuleFor(x => x.LastName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .MaximumLength(100).WithMessage(localizer["{PropertyName} can have a maximum of 100 characters."])
                .Must(IsOnlyLetterAndSpace).WithMessage(localizer["{PropertyName} should be all letters."])
                .NotEqual(x => x.FirstName).WithMessage(localizer["{PropertyName} cannot be equal to FirstName."]);
        }

        public bool IsOnlyLetterAndSpace(string value) => Regex.IsMatch(value, @"^(?! )[A-Za-z\s]+$");

        private bool IsOnlyNumber(string value) => value.All(char.IsNumber);
    }
}