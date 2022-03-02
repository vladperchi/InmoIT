// --------------------------------------------------------------------------------------------------
// <copyright file="RegisterUserRequestValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Linq;
using FluentValidation;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Shared.Dtos.Identity.Users;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Identity.Core.Validators
{
    public class RegisterUserRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterUserRequestValidator(IUserService userService, IStringLocalizer<RegisterUserRequestValidator> localizer)
        {
            RuleFor(x => x.Email).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .EmailAddress().WithMessage(localizer["{PropertyName} must be a valid email accounts."])
                .MustAsync(async (email, _) => !await userService.ExistsWithEmailAsync(email))
                    .WithMessage((_, email) => string.Format(localizer["Email {0} is already registered."], email));

            RuleFor(x => x.UserName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .MinimumLength(8)
                .MustAsync(async (name, _) => !await userService.ExistsWithNameAsync(name))
                    .WithMessage((_, name) => string.Format(localizer["Username {0} is already taken."], name));

            RuleFor(x => x.PhoneNumber).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Length(8, 16).WithMessage(localizer["{PropertyName} must have between 8 and  16 characters."])
                .Must(IsOnlyNumber).WithMessage(localizer["{PropertyName} should be all numbers."])
                .MustAsync(async (phone, _) => !await userService.ExistsWithPhoneNumberAsync(phone!))
                    .WithMessage((_, phone) => string.Format(localizer["Phone number {0} is already registered."], phone))
                    .Unless(u => string.IsNullOrWhiteSpace(u.PhoneNumber));

            RuleFor(x => x.FirstName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .MaximumLength(100).WithMessage(localizer["{PropertyName} can have a maximum of 100 characters."])
                .Must(IsOnlyLetter).WithMessage(localizer["{PropertyName} should be all letters."])
                .NotEqual(x => x.LastName).WithMessage(localizer["{PropertyName} cannot be equal to LastName."]);

            RuleFor(x => x.LastName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .MaximumLength(100).WithMessage(localizer["{PropertyName} can have a maximum of 100 characters."])
                .Must(IsOnlyLetter).WithMessage(localizer["{PropertyName} should be all letters."])
                .NotEqual(x => x.FirstName).WithMessage(localizer["{PropertyName} cannot be equal to FirstName."]);

            RuleFor(x => x.Password).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .MinimumLength(8).WithMessage(localizer["{PropertyName} minimum 8 characters."]);

            RuleFor(x => x.ConfirmPassword).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Equal(x => x.Password).WithMessage(localizer["{PropertyName} do not match."]);
        }

        private bool IsOnlyLetter(string value) => value.All(char.IsLetter);

        private bool IsOnlyNumber(string value) => value.All(char.IsNumber);
    }
}