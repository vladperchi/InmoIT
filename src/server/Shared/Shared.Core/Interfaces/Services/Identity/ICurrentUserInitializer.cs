// --------------------------------------------------------------------------------------------------
// <copyright file="ICurrentUserInitializer.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Security.Claims;

namespace InmoIT.Shared.Core.Interfaces.Services.Identity
{
    public interface ICurrentUserInitializer
    {
        void SetCurrentUserId(string userId);

        void SetCurrentUser(ClaimsPrincipal user);
    }
}