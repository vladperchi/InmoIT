// --------------------------------------------------------------------------------------------------
// <copyright file="HangfireAuthorizationFilter.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using Hangfire.Dashboard;
using InmoIT.Shared.Core.Constants;

namespace InmoIT.Shared.Infrastructure.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
           var currentUser = context.GetHttpContext().User;
           return currentUser.Identity.IsAuthenticated && currentUser.IsInRole(PermissionsConstant.Hangfire.View);
        }
    }
}