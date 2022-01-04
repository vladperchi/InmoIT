// --------------------------------------------------------------------------------------------------
// <copyright file="RemoveOwnerCommandValidator.cs" company="InmoIT">
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
    public class RemoveOwnerCommandValidator : AbstractValidator<RemoveOwnerCommand>
    {
        public RemoveOwnerCommandValidator(IStringLocalizer<RemoveOwnerCommandValidator> localizer)
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage(_ => localizer["{PropertyName} cannot be empty."]);
        }
    }
}