// --------------------------------------------------------------------------------------------------
// <copyright file="HeaderSettings.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Core.Settings
{
    public class HeaderSettings
    {
        public bool Enable { get; set; }

        public string XFrameOptions { get; set; }

        public string XContentTypeOptions { get; set; }

        public string ReferrerPolicy { get; set; }

        public string PermissionsPolicy { get; set; }

        public string SameSite { get; set; }

        public string XXSSProtection { get; set; }
    }
}