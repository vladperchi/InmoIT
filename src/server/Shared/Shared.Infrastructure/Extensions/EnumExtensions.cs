// --------------------------------------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace InmoIT.Shared.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescriptionString(this Enum value)
        {
            var type = value.GetType();
            if (!type.IsEnum) throw new ArgumentException($"Type '{type}' is not Enum");

            var attributes = (DescriptionAttribute[])value
                .GetType()
                .GetField(value.ToString())?
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes is { Length: > 0 }
                 ? attributes[0].Description
                 : value.ToString();
        }

        public static List<string> GetDescriptionList(this Enum val)
        {
            string result = val.ToDescriptionString();
            return result.Split('|').ToList();
        }
    }
}