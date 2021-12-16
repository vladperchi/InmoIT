// --------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

#nullable enable

namespace InmoIT.Shared.Core.Common
{
    public static class StringExtensions
    {
        public static string NullToString(this object? Value)
            => Value?.ToString() ?? string.Empty;
    }
}