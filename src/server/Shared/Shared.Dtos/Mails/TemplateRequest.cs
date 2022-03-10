// --------------------------------------------------------------------------------------------------
// <copyright file="TemplateRequest.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Dtos.Mails
{
    public class TemplateRequest
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Url { get; set; }

        public string Contact { get; set; }

        public string Team { get; set; }

        public string TeamUrl { get; set; }

        public string TermsUrl { get; set; }

        public string PrivacyUrl { get; set; }

        public string SupportUrl { get; set; }

        public string SendBy { get; set; }
    }
}