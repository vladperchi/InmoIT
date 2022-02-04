// --------------------------------------------------------------------------------------------------
// <copyright file="RemoveImageCommandValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.Images.Commands.Validators
{
    public class RemoveImageCommandValidator : AbstractValidator<RemoveImageCommand>
    {
        public RemoveImageCommandValidator(IStringLocalizer<RemoveImageCommandValidator> localizer)
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage(_ => localizer["{PropertyName} must not be empty."]);
        }
    }
}