// --------------------------------------------------------------------------------------------------
// <copyright file="IJsonSerializerOptions.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Text.Json;

namespace InmoIT.Shared.Core.Interfaces.Serialization.Options
{
    public interface IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; }
    }
}