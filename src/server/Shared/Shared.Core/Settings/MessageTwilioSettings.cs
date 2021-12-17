// --------------------------------------------------------------------------------------------------
// <copyright file="MessageTwilioSettings.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Core.Settings
{
    public class MessageTwilioSettings
    {
        public string MessageIdentification { get; set; }

        public string MessagePassword { get; set; }

        public string MessageFrom { get; set; }

        public bool MessageEnableVerification { get; set; }
    }
}