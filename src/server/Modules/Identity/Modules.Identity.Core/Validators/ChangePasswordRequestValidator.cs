// --------------------------------------------------------------------------------------------------
// <copyright file="ChangePasswordRequestValidator.cs" company="InmoIT">
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
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator(
            IStringLocalizer<ChangePasswordRequestValidator> localizer)
        {
            RuleFor(p => p.Password)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."]);
            RuleFor(p => p.NewPassword)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."]);
            RuleFor(p => p.ConfirmNewPassword)
                .NotEqual(p => p.NewPassword).WithMessage(localizer["{PropertyName} do not match."]);
        }
    }
}