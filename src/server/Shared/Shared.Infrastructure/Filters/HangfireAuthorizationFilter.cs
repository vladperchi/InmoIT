// --------------------------------------------------------------------------------------------------
// <copyright file="HangfireAuthorizationFilter.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using Hangfire.Dashboard;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Interfaces.Services.Identity;

namespace InmoIT.Shared.Infrastructure.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly ICurrentUser _user;

        public HangfireAuthorizationFilter(
            ICurrentUser user)
        {
            _user = user;
        }

        public bool Authorize(DashboardContext context)
        {
           return _user.IsAuthenticated() && _user.IsInRole(PermissionsConstant.Hangfire.View);
        }
    }
}