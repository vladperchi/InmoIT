// --------------------------------------------------------------------------------------------------
// <copyright file="ApplicationBuilderExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.IO;
using System.Runtime.CompilerServices;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Infrastructure.Middlewares;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

[assembly: InternalsVisibleTo("InmoIT.Api")]

namespace InmoIT.Shared.Infrastructure.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSharedInfrastructure(this IApplicationBuilder app, IConfiguration config)
        {
            app.UseExceptionMiddleware();
            app.UseSecurityHeaders(config);
            app.UseRouting();

            string filesDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (!Directory.Exists(filesDirectoryPath))
            {
                Directory.CreateDirectory(filesDirectoryPath);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
                RequestPath = new PathString("/Files")
            });
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "Handled {RequestPath}";
                options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                };
            });
            app.UseCorsPolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard(config);
            app.UseSwaggerDocumentation(config);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
            app.Initialize();

            return app;
        }

        internal static IApplicationBuilder Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            foreach (var initializer in serviceScope.ServiceProvider.GetServices<IDbSeeder>())
            {
                initializer.Initialize();
            }

            return app;
        }
    }
}