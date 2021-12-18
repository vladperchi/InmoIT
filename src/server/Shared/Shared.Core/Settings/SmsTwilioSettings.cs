// --------------------------------------------------------------------------------------------------
// <copyright file="SmsTwilioSettings.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Core.Settings
{
    public class SmsTwilioSettings
    {
        public string Identification { get; set; }

        public string Password { get; set; }

        public string From { get; set; }

        public bool EnableVerification { get; set; }
    }
}