// --------------------------------------------------------------------------------------------------
// <copyright file="UpdateImageCommandValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.Images.Commands.Validators
{
    public class UpdateImageCommandValidator : AbstractValidator<UpdateImageCommand>
    {
        public UpdateImageCommandValidator(
            IStringLocalizer<AddImageCommandValidator> localizer)
        {
            RuleFor(x => x.Id)
                  .NotEqual(Guid.Empty).WithMessage(_ => localizer["{PropertyName} must not be empty."]);
            RuleFor(x => x.PropertyId)
                  .NotEqual(Guid.Empty).WithMessage(_ => localizer["{PropertyName} must not be empty."]);
            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Length(10, 100).WithMessage(localizer["{PropertyName} must have between 10 and 100 characters."])
                .NotEqual(x => x.Caption).WithMessage(localizer["Cannot be equal to image caption."])
                .Must(HasExtension).WithMessage(localizer["{PropertyName} must have extension"]);
            RuleFor(x => x.Caption)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Length(50, 150).WithMessage(localizer["{PropertyName} must have between 50 and 150 characters."])
                .NotEqual(x => x.FileName).WithMessage(localizer["Cannot be equal to image file name."])
                .Must(IsOnlyLetter).WithMessage(localizer["{PropertyName} must be only letters."]);
            RuleFor(x => x.CodeImage)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .MaximumLength(10).WithMessage(localizer["{PropertyName} must have maximu 10 characters."])
                .Must(IsLetterOrDigit).WithMessage(localizer["{PropertyName} must be only letters and numbers."]);
        }

        private bool IsOnlyLetter(string value) => value.All(char.IsLetter);

        private bool IsLetterOrDigit(string value) => value.All(char.IsLetterOrDigit);

        private bool HasExtension(string value) => !string.IsNullOrWhiteSpace(Path.GetExtension(value));
    }
}