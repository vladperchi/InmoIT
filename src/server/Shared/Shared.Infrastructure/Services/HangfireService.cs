// --------------------------------------------------------------------------------------------------
// <copyright file="HangfireService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Interfaces.Services;
using Hangfire;

namespace InmoIT.Shared.Infrastructure.Services
{
    /// <inheritdoc cref = "IJobService" />
    public class HangfireService : IJobService
    {
        public string Enqueue(Expression<Func<Task>> methodCall) =>
            BackgroundJob.Enqueue(methodCall);
    }
}