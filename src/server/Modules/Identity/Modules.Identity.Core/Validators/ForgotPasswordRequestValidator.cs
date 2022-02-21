// --------------------------------------------------------------------------------------------------
// <copyright file="ForgotPasswordRequestValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using FluentValidation;
using InmoIT.Shared.Dtos.Identity.Users;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Identity.Core.Validators
{
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordRequestValidator(
            IStringLocalizer<ForgotPasswordRequestValidator> localizer)
        {
            RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .EmailAddress().WithMessage(localizer["{PropertyName} must be a valid email accounts."]);
        }
    }
}