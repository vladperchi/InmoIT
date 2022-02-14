// --------------------------------------------------------------------------------------------------
// <copyright file="SwaggerExcludeFilter.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using InmoIT.Shared.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InmoIT.Shared.Infrastructure.Swagger.Filters
{
    public class SwaggerExcludeFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context?.MethodInfo == null)
            {
                return;
            }

            var parameters = context.MethodInfo.GetParameters();
            var properties = parameters.SelectMany(x => x.ParameterType.GetProperties());
            var propertiesToRemove = properties
                .Where(p => p.GetCustomAttribute<SwaggerExcludeAttribute>() != null && p.GetCustomAttribute<FromQueryAttribute>() != null)
                .Select(p => p.Name)
                .ToHashSet(StringComparer.InvariantCultureIgnoreCase);

            foreach (var parameter in operation.Parameters.ToList())
            {
                if (propertiesToRemove.Contains(parameter.Name))
                {
                    operation.Parameters.Remove(parameter);
                }
            }
        }
    }
}