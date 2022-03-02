// --------------------------------------------------------------------------------------------------
// <copyright file="TokenUserRequest.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Dtos.Identity.Tokens
{
    public record TokenUserRequest(string Email, string Password);
}