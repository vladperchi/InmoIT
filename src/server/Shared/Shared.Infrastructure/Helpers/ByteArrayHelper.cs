// --------------------------------------------------------------------------------------------------
// <copyright file="ByteArrayHelper.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Infrastructure.Helpers
{
    public static class ByteArrayHelper
    {
        public static byte[] ToByteArray(this string value)
        {
            return System.Text.Encoding.ASCII.GetBytes(value);
        }
    }
}