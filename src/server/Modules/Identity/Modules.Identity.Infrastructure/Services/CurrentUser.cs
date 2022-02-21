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

        public string Name => _accessor.HttpContext?.User.Identity?.Name;

        public Guid GetUserId() => IsAuthenticated()
            ? Guid.Parse(_accessor.HttpContext?.User.GetUserId() ?? Guid.Empty.ToString())
            : Guid.Empty;

        public string GetUserEmail() => IsAuthenticated()
            ? _accessor.HttpContext?.User.GetUserEmail()
            : string.Empty;

        public bool IsAuthenticated() => _accessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

        public bool IsInRole(string role) => _accessor.HttpContext?.User.IsInRole(role) ?? false;

        public IEnumerable<Claim> GetUserClaims() => _accessor.HttpContext?.User.Claims;

        public HttpContext GetHttpContext() => _accessor.HttpContext;
    }
}