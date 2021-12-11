// --------------------------------------------------------------------------------------------------
// <copyright file="ValidationBehavior.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;

namespace InmoIT.Shared.Core.Behaviors
{
    public class ValidationBehavior
    {
    }

    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly IStringLocalizer<ValidationBehavior> _localizer;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, IStringLocalizer<ValidationBehavior> localizer)
        {
            _validators = validators;
            _localizer = localizer;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    var errorMessages = failures.Select(a => a.ErrorMessage).Distinct().ToList();
                    throw new ValidationCustomException(_localizer, errorMessages);
                }
            }

            return await next();
        }
    }
}