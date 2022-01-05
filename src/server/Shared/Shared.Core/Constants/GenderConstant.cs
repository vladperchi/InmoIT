// --------------------------------------------------------------------------------------------------
// <copyright file="GenderConstant.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace InmoIT.Shared.Core.Constants
{
    public static class GenderConstant
    {
        [DisplayName("GenderType")]
        [Description("Gender Type Users")]
        public static class GenderType
        {
            public const string Female = "Female";
            public const string Male = "Male";
        }
    }
}