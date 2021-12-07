// --------------------------------------------------------------------------------------------------
// <copyright file="PermissionRequest.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace InmoIT.Shared.Dtos.Identity.Roles
{
    public class PermissionRequest
    {
        public string RoleId { get; set; }

        public IList<RoleClaimModel> RoleClaims { get; set; }
    }
}