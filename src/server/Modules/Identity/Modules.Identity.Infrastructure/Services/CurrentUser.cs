// --------------------------------------------------------------------------------------------------
// <copyright file="CurrentUser.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security.Claims;
using InmoIT.Modules.Identity.Infrastructure.Extensions;
using InmoIT.Shared.Core.Interfaces.Services.Identity;
using Microsoft.AspNetCore.Http;

namespace InmoIT.Modules.Identity.Infrastructure.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name =>
            GetHttpContext()?.User.Identity?.Name;

        public Guid GetUserId() =>
            IsAuthenticated()
            ? Guid.Parse(GetHttpContext()?.User.GetUserId() ?? Guid.Empty.ToString())
            : Guid.Empty;

        public string GetUserEmail() =>
            IsAuthenticated()
            ? GetHttpContext()?.User.GetUserEmail()
            : string.Empty;

        public string GetUserPhoneNumber() =>
            IsAuthenticated()
            ? GetHttpContext()?.User.GetUserPhoneNumber()
            : string.Empty;

        public string GetUserFirstName() =>
            IsAuthenticated()
            ? GetHttpContext()?.User.GetUserFirstName()
            : string.Empty;

        public string GetuserSurname() =>
            IsAuthenticated()
            ? GetHttpContext()?.User.GetuserSurname()
            : string.Empty;

        public bool IsAuthenticated() =>
            GetHttpContext()?.User.Identity?.IsAuthenticated ?? false;

        public bool IsInRole(string role) =>
            GetHttpContext()?.User.IsInRole(role) ?? false;

        public IEnumerable<Claim> GetUserClaims() =>
            GetHttpContext()?.User.Claims;

        public HttpContext GetHttpContext() =>
            _accessor.HttpContext;
    }
}