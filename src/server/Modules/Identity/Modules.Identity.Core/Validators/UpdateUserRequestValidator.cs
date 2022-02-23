// --------------------------------------------------------------------------------------------------
// <copyright file="UpdateUserRequestValidator.cs" company="InmoIT">
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
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator(IIdentityService identityService, IStringLocalizer<UpdateUserRequestValidator> localizer)
        {
            RuleFor(x => x.Id)
                  .NotEqual(string.Empty).WithMessage(_ => localizer["{PropertyName} must not be empty."]);

            RuleFor(p => p.Email)
            .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
            .EmailAddress().WithMessage(localizer["{PropertyName} must be a valid email accounts."])
            .MustAsync(async (user, email, _) => !await identityService.ExistsWithEmailAsync(email, user.Id))
                .WithMessage((_, email) => string.Format(localizer["Email {0} is already registered."], email));

            RuleFor(u => u.PhoneNumber)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Length(8, 16).WithMessage(localizer["{PropertyName} must have between 8 and  16 characters."])
                .Must(IsOnlyNumber).WithMessage(localizer["{PropertyName} should be all numbers."])
                .MustAsync(async (user, phone, _) => !await identityService.ExistsWithPhoneNumberAsync(phone, user.Id))
                    .WithMessage((_, phone) => string.Format(localizer["Phone number {0} is already registered."], phone))
                    .Unless(u => string.IsNullOrWhiteSpace(u.PhoneNumber));

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .MaximumLength(100).WithMessage(localizer["{PropertyName} can have a maximum of 100 characters."])
                .Must(IsOnlyLetter).WithMessage(localizer["{PropertyName} should be all letters."])
                .NotEqual(x => x.LastName).WithMessage(localizer["{PropertyName} cannot be equal to LastName."]);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .MaximumLength(100).WithMessage(localizer["{PropertyName} can have a maximum of 100 characters."])
                .Must(IsOnlyLetter).WithMessage(localizer["{PropertyName} should be all letters."])
                .NotEqual(x => x.FirstName).WithMessage(localizer["{PropertyName} cannot be equal to FirstName."]);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .MinimumLength(8);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Equal(x => x.Password).WithMessage(localizer["{PropertyName} do not match."]);
        }

        private bool IsOnlyLetter(string value) => value.All(char.IsLetter);

        private bool IsOnlyNumber(string value) => value.All(char.IsNumber);
    }
}