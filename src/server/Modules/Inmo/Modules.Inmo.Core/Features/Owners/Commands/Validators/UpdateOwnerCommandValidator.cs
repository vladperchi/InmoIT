// --------------------------------------------------------------------------------------------------
// <copyright file="UpdateOwnerCommandValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.Owners.Commands.Validators
{
    public class UpdateOwnerCommandValidator : AbstractValidator<UpdateOwnerCommand>
    {
        public UpdateOwnerCommandValidator(IStringLocalizer<UpdateOwnerCommandValidator> localizer)
        {
            RuleFor(c => c.Id)
                  .NotEqual(Guid.Empty).WithMessage(_ => localizer["{PropertyName} cannot be empty."]);
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage(localizer["{PropertyName} cannot be empty."])
                .Length(8, 50).WithMessage(localizer["{PropertyName} must have between 8 and 50 characters."]);
            RuleFor(c => c.SurName)
                .NotEmpty().WithMessage(localizer["{PropertyName} cannot be empty."])
                .Length(8, 50).WithMessage(localizer["{PropertyName} must have between 8 and 50 characters."]);
            RuleFor(c => c.Address)
                .NotEmpty().WithMessage(localizer["{PropertyName} cannot be empty."])
                .Length(8, 100).WithMessage(localizer["{PropertyName} must have between 20 and 100 characters."]);
            RuleFor(c => c.Email)
               .NotEmpty().WithMessage(localizer["{PropertyName} cannot be empty."]);
            RuleFor(c => c.PhoneNumber)
                .NotEmpty().WithMessage(localizer["{PropertyName} cannot be empty."])
                .Length(8, 16).WithMessage(localizer["{PropertyName} must have between 8 and  16 characters."]);
            RuleFor(c => c.Birthday)
               .NotEmpty().WithMessage(localizer["{PropertyName} cannot be empty."]);
            RuleFor(c => c.Gender)
              .NotEmpty().WithMessage(localizer["{PropertyName} cannot be empty."]);
            RuleFor(c => c.Group)
                .NotEmpty().WithMessage(localizer["{PropertyName} cannot be empty."]);
        }
    }
}