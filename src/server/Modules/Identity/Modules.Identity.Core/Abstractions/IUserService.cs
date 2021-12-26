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

        Task<IResult<UserRolesResponse>> GetRolesAsync(string userId);

        Task<IResult<string>> UpdateAsync(UpdateUserRequest request);

        Task<IResult<string>> UpdateUserRolesAsync(string userId, UserRolesRequest request);

        Task<string> ExportToExcelAsync(string searchString = "");

        Task<int> GetCountAsync();
    }
}