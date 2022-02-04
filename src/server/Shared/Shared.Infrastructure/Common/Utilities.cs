// --------------------------------------------------------------------------------------------------
// <copyright file="Utilities.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InmoIT.Shared.Infrastructure.Common
{
    public static class Utilities
    {
        private static readonly Random Random = new Random();

        public static List<T> GetAllConstantValues<T>(this Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
                .Select(x => (T)x.GetRawConstantValue())
                .ToList();
        }

        public static List<string> GetNestedStringValues(this Type type)
        {
            var values = new List<string>();
            foreach (var prop in type.GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                object propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                {
                    values.Add(propertyValue.ToString());
                }
            }

            return values;
        }

        public static Task<string> GenerateCode(string letter, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return Task.FromResult(new string(Enumerable
                .Repeat($"{letter.ToUpper()}-{chars}INMO", length)
                .Select(s => s[Random.Next(s.Length)])
                .ToArray()));
        }
    }
}