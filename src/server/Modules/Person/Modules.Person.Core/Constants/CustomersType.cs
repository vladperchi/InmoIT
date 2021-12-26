// --------------------------------------------------------------------------------------------------
// <copyright file="CustomersType.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace InmoIT.Modules.Person.Core.Constants
{
    public static class CustomersType
    {
        [DisplayName("Gender Type")]
        [Description("Gender Type CustomersType")]
        public static class GenderType
        {
            public const string Female = "Female";
            public const string Male = "Male";
        }

        [DisplayName("Group Type")]
        [Description("Group Type CustomersType")]
        public static class GroupType
        {
            public const string VIP = "VIP";
            public const string Normal = "Normal";
        }
    }
}