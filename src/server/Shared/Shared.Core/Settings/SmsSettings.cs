// --------------------------------------------------------------------------------------------------
// <copyright file="SmsSettings.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Core.Settings
{
    public class SmsSettings
    {
        public string SmsIdentification { get; set; }

        public string SmsPassword { get; set; }

        public string SmsFrom { get; set; }

        public bool EnableVerification { get; set; }
    }
}