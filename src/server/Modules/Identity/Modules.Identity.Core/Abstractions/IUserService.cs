// --------------------------------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Identity.Users;

namespace InmoIT.Modules.Identity.Core.Abstractions
{
    public interface IUserService
    {
        Task<Result<List<UserResponse>>> GetAllAsync();

        Task<IResult<UserResponse>> GetByIdAsync(string userId);

        Task<IResult<string>> GetPictureAsync(string userId);

        Task<IResult<UserRolesResponse>> GetRolesByUserAsync(string userId);

        Task<IResult> RegisterAsync(RegisterRequest request, string origin);

        Task<IResult<string>> UpdateAsync(UpdateUserRequest request);

        Task<IResult<string>> UpdatePictureAsync(string userId, UpdateUserPictureRequest request);

        Task<IResult<string>> UpdateRolesByUserAsync(string userId, UserRolesRequest request);

        Task<IResult<string>> ConfirmEmailAsync(string userId, string code);

        Task<IResult<string>> ConfirmPhoneNumberAsync(string userId, string code);

        Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);

        Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);

        Task<IResult> ChangePasswordAsync(string userId, ChangePasswordRequest request);

        Task<Result<string>> ExportAsync(string searchString = "");

        Task<Result<string>> DeleteAsync(string userId);

        Task<bool> ExistsWithNameAsync(string name);

        Task<bool> ExistsWithEmailAsync(string email, string exceptId = null);

        Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string exceptId = null);

        Task<int> GetCountAsync();
    }
}