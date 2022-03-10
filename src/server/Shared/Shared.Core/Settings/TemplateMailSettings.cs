// --------------------------------------------------------------------------------------------------
// <copyright file="TemplateMailSettings.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Core.Settings
{
    public class TemplateMailSettings
    {
        public bool Enable { get; set; }

        public string Contact { get; set; }

        public string TeamName { get; set; }

        public string TeamUrl { get; set; }

        public string TermsUrl { get; set; }

        public string PrivacyUrl { get; set; }

        public string SupportUrl { get; set; }

        public string SendBy { get; set; }
    }
}