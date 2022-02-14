// --------------------------------------------------------------------------------------------------
// <copyright file="HavePermission.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;

namespace InmoIT.Shared.Infrastructure.Permissions
{
    /// <inheritdoc cref = "AuthorizeAttribute" />
    public class HavePermission : AuthorizeAttribute
    {
        public HavePermission(string permission)
        {
            Policy = permission;
        }
    }
}