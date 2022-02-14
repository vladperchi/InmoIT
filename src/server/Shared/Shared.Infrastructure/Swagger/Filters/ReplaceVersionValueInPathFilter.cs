// --------------------------------------------------------------------------------------------------
// <copyright file="ReplaceVersionValueInPathFilter.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InmoIT.Shared.Infrastructure.Swagger.Filters
{
    public class ReplaceVersionValueInPathFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = new OpenApiPaths();

            foreach (var (key, value) in swaggerDoc.Paths)
                paths.Add(key.Replace("v{version}", swaggerDoc.Info.Version, StringComparison.InvariantCultureIgnoreCase), value);

            swaggerDoc.Paths = paths;
        }
    }
}