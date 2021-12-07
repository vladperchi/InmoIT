// --------------------------------------------------------------------------------------------------
// <copyright file="MailRequest.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Dtos.Mails
{
    public class MailRequest
    {
        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string From { get; set; }
    }
}