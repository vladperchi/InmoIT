// --------------------------------------------------------------------------------------------------
// <copyright file="HangfireJobFilter.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Security.Claims;
using Hangfire.Client;
using Hangfire.Logging;
using InmoIT.Shared.Core.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace InmoIT.Shared.Infrastructure.HangfireJobs
{
    public class HangfireJobFilter : IClientFilter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        private readonly IServiceProvider _services;

        public HangfireJobFilter(IServiceProvider services) => _services = services;

        public void OnCreating(CreatingContext context)
        {
            _ = context ?? throw new ArgumentNullException("Received a null argument!");
            Logger.InfoFormat("Set UserId parameters to job {0}.{1}...", context.Job.Method.ReflectedType?.FullName, context.Job.Method.Name);
            using var scope = _services.CreateScope();
            var httpContext = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext;
            _ = httpContext ?? throw new InvalidOperationException("To create a job there must be exist HttpContext.");
            string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            context.SetJobParameter(StringKeys.UserId, userId);
        }

        public void OnCreated(CreatedContext context) =>
            Logger.InfoFormat("Job created with parameters {0}", context.Parameters
                .Select(x => x.Key + "=" + x.Value)
                .Aggregate((s1, s2) => s1 + ";" + s2));
    }
}