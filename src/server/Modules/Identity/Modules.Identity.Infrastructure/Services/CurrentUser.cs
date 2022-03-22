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
using InmoIT.Modules.Identity.Core.Exceptions;
using InmoIT.Modules.Identity.Infrastructure.Extensions;
using InmoIT.Shared.Core.Interfaces.Services.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Identity.Infrastructure.Services
{
    public class CurrentUser : ICurrentUser, ICurrentUserInitializer
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IStringLocalizer<CurrentUser> _localizer;
        private ClaimsPrincipal _user;
        private Guid _userId = Guid.Empty;

        public CurrentUser(
            IHttpContextAccessor accessor,
            IStringLocalizer<CurrentUser> localizer)
        {
            _accessor = accessor;
            _localizer = localizer;
        }

        public string Name => _user.Identity?.Name;

        public Guid GetUserId() =>
            IsAuthenticated() ? Guid.Parse(_user.GetUserId() ?? Guid.Empty.ToString()) : _userId;

        public string GetUserEmail() =>
            IsAuthenticated() ? _user.GetUserEmail() : string.Empty;

        public string GetUserPhoneNumber() =>
            IsAuthenticated() ? _user.GetUserPhoneNumber() : string.Empty;

        public string GetUserFirstName() =>
            IsAuthenticated() ? _user.GetUserFirstName() : string.Empty;

        public string GetuserSurname() =>
            IsAuthenticated() ? _user.GetuserSurname() : string.Empty;

        public string GetFullName() =>
            IsAuthenticated() ? _user.GetFullName() : string.Empty;

        public string GetImageUrl() =>
            IsAuthenticated() ? _user.GetImageUrl() : string.Empty;

        public bool IsAuthenticated() =>
            _user?.Identity?.IsAuthenticated is true;

        public bool IsInRole(string role) =>
            _user?.IsInRole(role) is true;

        public IEnumerable<Claim> GetUserClaims() =>
            _user?.Claims;

        public HttpContext GetHttpContext() =>
            _accessor.HttpContext;

        public void SetCurrentUserId(string userId)
        {
            if (_userId != Guid.Empty)
            {
                throw new ReservedCustomException(_localizer, null);
            }

            if (!string.IsNullOrEmpty(userId))
            {
                _userId = Guid.Parse(userId);
            }
        }
    }
}