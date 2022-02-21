// --------------------------------------------------------------------------------------------------
// <copyright file="UpdatePropertyCommandValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.Properties.Commands.Validators
{
    public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
    {
        public UpdatePropertyCommandValidator(
            IStringLocalizer<UpdatePropertyCommandValidator> localizer)
        {
            RuleFor(x => x.Id)
                 .NotEqual(Guid.Empty).WithMessage(_ => localizer["{PropertyName} must not be empty."]);
            RuleFor(x => x.OwnerId)
                  .NotEqual(Guid.Empty).WithMessage(_ => localizer["{PropertyName} must not be empty."]);
            RuleFor(x => x.PropertyTypeId)
                  .NotEqual(Guid.Empty).WithMessage(_ => localizer["{PropertyName} must not be empty."]);
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
               .Length(20, 150).WithMessage(localizer["{PropertyName} property must have between 10 and 100 characters."])
               .Must(IsOnlyLetter).WithMessage(localizer["{PropertyName} should be all letters."])
               .NotEqual(x => x.Address).WithMessage(localizer["{PropertyName} cannot be equal to Address."])
               .NotEqual(x => x.Description).WithMessage(localizer["{PropertyName} cannot be equal to Description."])
               .NotEqual(x => x.CodeInternal).WithMessage(localizer["{PropertyName} cannot be equal to code internal."]);
            RuleFor(x => x.Address)
               .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
               .Length(20, 150).WithMessage(localizer["{PropertyName} must have between 20 and 150 characters."])
               .NotEqual(x => x.Name).WithMessage(localizer["{PropertyName} cannot be equal to Name."])
               .NotEqual(x => x.Description).WithMessage(localizer["{PropertyName} cannot be equal to Description."])
               .NotEqual(x => x.CodeInternal).WithMessage(localizer["{PropertyName} cannot be equal to code internal."]);
            RuleFor(x => x.Description)
               .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
               .Length(50, 250).WithMessage(localizer["{PropertyName} must have between 50 and 250 characters."])
               .Must(IsLetterOrDigit).WithMessage(localizer["{PropertyName} must be only letters and numbers."])
               .NotEqual(x => x.Name).WithMessage(localizer["{PropertyName} cannot be equal to Name."])
               .NotEqual(x => x.Address).WithMessage(localizer["{PropertyName} cannot be equal to Address."])
               .NotEqual(x => x.CodeInternal).WithMessage(localizer["{PropertyName} cannot be equal to code internal."]);
            RuleFor(x => x.SalePrice)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Must(IsOnlyDigit).WithMessage(localizer["{PropertyName} should be all decimal."])
                .GreaterThan(0).WithMessage(localizer["{PropertyName} must be greater than 0"]);
            RuleFor(x => x.RentPrice)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Must(IsOnlyDigit).WithMessage(localizer["{PropertyName} should be all decimal."])
                .GreaterThan(0).WithMessage(localizer["{PropertyName} must be greater than 0"]);
            RuleFor(x => x.SaleTax)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Must(IsOnlyDigit).WithMessage(localizer["{PropertyName} should be all decimal."])
                .GreaterThan(0).WithMessage(localizer["{PropertyName} must be greater than 0"]);
            RuleFor(x => x.IncomeTax)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Must(IsOnlyDigit).WithMessage(localizer["{PropertyName} should be all decimal."])
                .GreaterThan(0).WithMessage(localizer["{PropertyName} must be greater than 0"]);
            RuleFor(x => x.CodeInternal)
               .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
               .MaximumLength(10).WithMessage(localizer["{PropertyName} must have maximu 10 characters."])
               .NotEqual(x => x.Name).WithMessage(localizer["{PropertyName} cannot be equal to Name."])
               .NotEqual(x => x.Address).WithMessage(localizer["{PropertyName} cannot be equal to Address."])
               .NotEqual(x => x.Description).WithMessage(localizer["{PropertyName} cannot be equal to Description."])
               .Must(IsLetterOrDigit).WithMessage(localizer["{PropertyName} must be only letters and numbers."]);
            RuleFor(x => x.Year)
                .NotEmpty().WithMessage(localizer["{PropertyName} must not be empty."])
                .Must(IsOnlyNumber).WithMessage(localizer["{PropertyName} must be only numbers."]);
        }

        private bool IsOnlyLetter(string value) => value.All(char.IsLetter);

        private bool IsOnlyDigit(decimal value) => value.ToString().All(char.IsDigit);

        private bool IsOnlyNumber(int value) => value.ToString().All(char.IsNumber);

        private bool IsLetterOrDigit(string value) => value.All(char.IsLetterOrDigit);
    }
}