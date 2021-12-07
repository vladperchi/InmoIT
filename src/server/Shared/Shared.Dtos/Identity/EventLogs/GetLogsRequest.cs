// --------------------------------------------------------------------------------------------------
// <copyright file="GetLogsRequest.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

#nullable enable
using System;

namespace InmoIT.Shared.Dtos.Identity.EventLogs
{
    public class GetLogsRequest
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string? SearchString { get; set; }

        public string[]? OrderBy { get; set; }

        public Guid UserId { get; set; }

        public string? Email { get; set; }

        public string? MessageType { get; set; }
    }
}