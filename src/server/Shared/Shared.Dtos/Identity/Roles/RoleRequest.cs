﻿// --------------------------------------------------------------------------------------------------
// <copyright file="RoleRequest.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Dtos.Identity.Roles
{
    public class RoleRequest
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}