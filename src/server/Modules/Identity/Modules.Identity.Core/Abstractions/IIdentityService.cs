// --------------------------------------------------------------------------------------------------
// <copyright file="IIdentityService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Identity.Users;

namespace InmoIT.Modules.Identity.Core.Abstractions
{
    public interface IIdentityService
    {
        Task<IResult> RegisterAsync(RegisterRequest request, string origin);

        Task<IResult<string>> ConfirmEmailAsync(string userId, string code);

        Task<IResult<string>> ConfirmPhoneNumberAsync(string userId, string code);

        Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);

        Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);

        Task<IResult<string>> GetUserPictureAsync(string userId);

        Task<IResult<string>> UpdateUserPictureAsync(UpdateUserPictureRequest request, string userId);
    }
}