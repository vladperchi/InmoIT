// --------------------------------------------------------------------------------------------------
// <copyright file="MvcBuilderExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Reflection;
using InmoIT.Modules.Person.Core.Abstractions;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace InmoIT.Modules.Person.Infrastructure.Extensions
{
    internal static class MvcBuilderExtensions
    {
        internal static IMvcBuilder AddCustomerValidation(this IMvcBuilder builder)
        {
            return builder.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(ICustomerDbContext))));
        }
    }
}