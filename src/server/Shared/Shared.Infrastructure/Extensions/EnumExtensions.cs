// --------------------------------------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;

namespace InmoIT.Shared.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescriptionString(this Enum val)
        {
            var attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString())?.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes is { Length: > 0 }
                 ? attributes[0].Description
                 : val.ToString();
        }
    }
}