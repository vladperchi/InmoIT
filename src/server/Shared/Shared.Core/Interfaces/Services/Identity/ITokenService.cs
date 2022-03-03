// --------------------------------------------------------------------------------------------------
// <copyright file="ITokenService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Identity.Tokens;

namespace InmoIT.Shared.Core.Interfaces.Services.Identity
{
    public interface ITokenService
    {
        Task<IResult<TokenUserResponse>> GetTokenAsync(TokenUserRequest request, string ipAddress);

        Task<IResult<TokenUserResponse>> RefreshTokenAsync(RefreshTokenUserRequest request, string ipAddress);
    }
}