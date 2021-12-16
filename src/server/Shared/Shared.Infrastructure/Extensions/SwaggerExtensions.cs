// --------------------------------------------------------------------------------------------------
// <copyright file="SwaggerExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Infrastructure.Swagger.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace InmoIT.Shared.Infrastructure.Extensions
{
    public static class SwaggerExtensions
    {
        internal static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IConfiguration config)
        {
            if (config.GetValue<bool>("SwaggerSettings:Enable"))
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.DefaultModelsExpandDepth(-1);
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = "swagger";
                    options.DisplayRequestDuration();
                    options.DocExpansion(DocExpansion.None);
                });
            }

            return app;
        }

        internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            var settings = services.GetOptions<SwaggerSettings>(nameof(SwaggerSettings));
            if (settings.Enable)
            {
                services.AddSwaggerGen(options =>
                {
                    options.AddSwaggerDocs();
                    options.OperationFilter<RemoveVersionParameterFilter>();
                    options.DocumentFilter<ReplaceVersionValueInPathFilter>();
                    options.OperationFilter<SwaggerExcludeFilter>();
                    options.DocInclusionPredicate((version, desc) =>
                    {
                        if (!desc.TryGetMethodInfo(out var methodInfo))
                        {
                            return false;
                        }

                        var versions = methodInfo
                            .DeclaringType?
                            .GetCustomAttributes(true)
                            .OfType<ApiVersionAttribute>()
                            .SelectMany(attr => attr.Versions);

                        var maps = methodInfo
                            .GetCustomAttributes(true)
                            .OfType<MapToApiVersionAttribute>()
                            .SelectMany(attr => attr.Versions)
                            .ToList();

                        return versions?.Any(v => $"v{v}" == version) == true
                               && (!maps.Any() || maps.Any(v => $"v{v}" == version));
                    });

                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        if (!assembly.IsDynamic)
                        {
                            string xmlFile = $"{assembly.GetName().Name}.xml";
                            string xmlPath = Path.Combine(baseDirectory, xmlFile);
                            if (File.Exists(xmlPath))
                            {
                                options.IncludeXmlComments(xmlPath);
                            }
                        }
                    }

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer",
                                },
                                Scheme = "Bearer",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            }, new List<string>()
                        },
                    });

                    options.MapType<TimeSpan>(() => new OpenApiSchema
                    {
                        Type = "string",
                        Nullable = true,
                        Pattern = @"^([0-9]{1}|(?:0[0-9]|1[0-9]|2[0-3])+):([0-5]?[0-9])(?::([0-5]?[0-9])(?:.(\d{1,9}))?)?$",
                        Example = new OpenApiString("02:00:00")
                    });
                    options.OperationFilter<SwaggerHeaderFilter>();
                });
            }

            return services;
        }

        private static void AddSwaggerDocs(this SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "InmoIT.API",
                Description = "Modular Clean Architecture built in ASP.NET Core 5.0 WebAPI.",
                Contact = new OpenApiContact
                {
                    Name = "Vladimir P. CHibás",
                    Email = "codewithvladperchi@outlook.com",
                    Url = new Uri("https://twitter.com/codewithvlad"),
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://github.com/vladperchi/InmoIT/blob/master/LICENSE"),
                }
            });

            // Here you can set future versions
        }
    }
}