// --------------------------------------------------------------------------------------------------
// <copyright file="AddImageCommandValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using FluentValidation;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.Images.Commands.Validators
{
    public class AddImageCommandValidator : AbstractValidator<AddImageCommand>
    {
        public AddImageCommandValidator(
            IStringLocalizer<AddImageCommandValidator> localizer)
        {
            RuleFor(x => x.PropertyId)
                  .NotEqual(Guid.Empty).WithMessage(_ => localizer["{PropertyName} must not be empty."]);

            When(x => x.PropertyImageList != null, () =>
            {
                RuleFor(x => x.PropertyImageList[0].FileName)
                  .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                  .Must(HasExtension).WithMessage(localizer["{PropertyName} must have extension"]);
                RuleFor(x => x.PropertyImageList[1].Caption)
                  .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                  .Length(50, 150).WithMessage(localizer["{PropertyName} must have between 50 and 150 characters."])
                  .NotEqual(x => x.PropertyImageList[2].CodeImage).WithMessage(localizer["Cannot be equal to code image."])
                  .Must(IsOnlyLetterAndSpace).WithMessage(localizer["{PropertyName} must be only letters."]);
                RuleFor(x => x.PropertyImageList[2].CodeImage)
                  .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                  .MaximumLength(10).WithMessage(localizer["{PropertyName} must have maximu 10 characters."])
                  .NotEqual(x => x.PropertyImageList[1].Caption).WithMessage(localizer["Cannot be equal to image caption."])
                  .Must(IsLetterOrDigit).WithMessage(localizer["{PropertyName} must be only letters and numbers."]);
            });
        }

        public bool IsOnlyLetterAndSpace(string value) => Regex.IsMatch(value, @"^(?! )[A-Za-z\s]+$");

        private bool IsLetterOrDigit(string value) => value.All(char.IsLetterOrDigit);

        private bool HasExtension(string value) => !string.IsNullOrWhiteSpace(Path.GetExtension(value));
    }
}