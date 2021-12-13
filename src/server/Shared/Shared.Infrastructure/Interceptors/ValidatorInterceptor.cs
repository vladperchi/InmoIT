// --------------------------------------------------------------------------------------------------
// <copyright file="ValidatorInterceptor.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Linq;
using InmoIT.Shared.Core.Exceptions;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace InmoIT.Shared.Infrastructure.Interceptors
{
    public class ValidatorInterceptor : IValidatorInterceptor
    {
        private readonly IStringLocalizer<ValidatorInterceptor> _localizer;

        public ValidatorInterceptor(IStringLocalizer<ValidatorInterceptor> localizer)
        {
            _localizer = localizer;
        }

        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }

        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            var failures = result.Errors.Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                var errorMessages = failures.Select(a => a.ErrorMessage).Distinct().ToList();
                throw new ValidationCustomException(_localizer, errorMessages);
            }

            return result;
        }
    }
}