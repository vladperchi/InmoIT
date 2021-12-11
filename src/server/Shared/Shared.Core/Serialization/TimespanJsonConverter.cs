// --------------------------------------------------------------------------------------------------
// <copyright file="TimespanJsonConverter.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace InmoIT.Shared.Core.Serialization
{
    public class TimespanJsonConverter : JsonConverter<TimeSpan>
    {
        public const string TimeSpanFormatString = @"d\.hh\:mm\:ss\:FFF";

        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string s = reader.GetString();
            if (string.IsNullOrWhiteSpace(s))
            {
                return TimeSpan.Zero;
            }

            if (!TimeSpan.TryParseExact(s, TimeSpanFormatString, null, out var parsedTimeSpan))
            {
                throw new FormatException($"Unexpected input timespan format : expected {Regex.Unescape(TimeSpanFormatString)}. Retrieve this key as a string and review manually.");
            }

            return parsedTimeSpan;
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            string timespanFormatted = $"{value.ToString(TimeSpanFormatString)}";
            writer.WriteStringValue(timespanFormatted);
        }
    }
}