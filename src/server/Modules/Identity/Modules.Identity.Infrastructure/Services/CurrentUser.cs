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

        private ClaimsPrincipal _user;

        private Guid _userId = Guid.Empty;

        public string Name => _user?.Identity?.Name;

        public Guid GetUserId() => IsAuthenticated() ? Guid.Parse(_user?.GetUserId() ?? Guid.Empty.ToString()) : _userId;

        public string GetUserEmail() => IsAuthenticated() ? _user?.GetUserEmail() : string.Empty;

        public string GetUserPhoneNumber() => IsAuthenticated() ? _user?.GetUserPhoneNumber() : string.Empty;

        public string GetUserFirstName() => IsAuthenticated() ? _user?.GetUserFirstName() : string.Empty;

        public string GetuserSurname() => IsAuthenticated() ? _user?.GetuserSurname() : string.Empty;

        public bool IsAuthenticated() => _user?.Identity?.IsAuthenticated ?? false;

        public bool IsInRole(string role) => _user?.IsInRole(role) ?? false;

        public IEnumerable<Claim> GetUserClaims() => _user?.Claims;

        public HttpContext GetHttpContext() => _accessor.HttpContext;
    }
}