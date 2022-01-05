// --------------------------------------------------------------------------------------------------
// <copyright file="GroupConstant.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace InmoIT.Shared.Core.Constants
{
    public static class GroupConstant
    {
        [DisplayName("GroupType")]
        [Description("Group Type Users")]
        public static class GroupType
        {
            public const string VIP = "VIP";
            public const string Normal = "Normal";
        }
    }
}