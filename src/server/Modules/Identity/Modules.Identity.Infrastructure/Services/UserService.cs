// --------------------------------------------------------------------------------------------------
// <copyright file="UserService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Modules.Identity.Core.Exceptions;
using InmoIT.Modules.Identity.Core.Features.Users.Events;
using InmoIT.Modules.Identity.Infrastructure.Specifications;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Interfaces.Services.Identity;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Identity.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Identity.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<InmoUser> _userManager;
        private readonly RoleManager<InmoRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UserService> _localizer;
        private readonly IExcelService _excelService;
        private readonly ILoggerService _eventLog;
        private readonly ICurrentUser _currentUser;

        public UserService(
            UserManager<InmoUser> userManager,
            RoleManager<InmoRole> roleManager,
            IMapper mapper,
            IStringLocalizer<UserService> localizer,
            IExcelService excelService,
            ILoggerService eventLog,
            ICurrentUser currentUser)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _localizer = localizer;
            _excelService = excelService;
            _eventLog = eventLog;
            _currentUser = currentUser;
        }

        public async Task<Result<List<UserResponse>>> GetAllAsync()
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            if (users == null)
            {
                throw new UserListEmptyException(_localizer);
            }

            try
            {
                var mapperUsers = _mapper.Map<List<UserResponse>>(users);
                return await Result<List<UserResponse>>.SuccessAsync(mapperUsers);
            }
            catch (Exception)
            {
                throw new IdentityCustomException(_localizer, null);
            }
        }

        public async Task<IResult<UserResponse>> GetByIdAsync(string userId)
        {
            var user = await _userManager.Users.AsNoTracking().Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new UserNotFoundException(_localizer);
            }

            try
            {
                var mapperUser = _mapper.Map<UserResponse>(user);
                return await Result<UserResponse>.SuccessAsync(mapperUser);
            }
            catch (Exception)
            {
                throw new IdentityCustomException(_localizer, null);
            }
        }

        public async Task<IResult<UserRolesResponse>> GetRolesAsync(string userId)
        {
            var viewModel = new List<UserRoleModel>();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(_localizer);
            }

            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
            foreach (var role in roles)
            {
                var userRolesViewModel = new UserRoleModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }

                viewModel.Add(userRolesViewModel);
            }

            var result = new UserRolesResponse { UserRoles = viewModel };
            return await Result<UserRolesResponse>.SuccessAsync(result);
        }

        public async Task<IResult<string>> UpdateAsync(UpdateUserRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                var userWithPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
                if (userWithPhoneNumber != null)
                {
                    throw new IdentityException(string.Format(_localizer["Phone number {0} is registered."], request.PhoneNumber));
                }
            }

            var user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                throw new IdentityException(string.Format(_localizer["User Id {0} is not found."], request.Id));
            }

            user.AddDomainEvent(new UserUpdatedEvent(user));

            if (request.CurrentPassword != null && request.Password != null)
            {
                await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.Password);
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return await Result<string>.SuccessAsync(user.Id, _localizer["User Updated Succesffull."]);
            }
            else
            {
                var errorMessages = result.Errors.Select(a => _localizer[a.Description].ToString()).Distinct().ToList();
                throw new IdentityCustomException(_localizer, errorMessages);
            }
        }

        public async Task<IResult<string>> UpdateUserRolesAsync(string userId, UserRolesRequest request)
        {
            var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new UserNotFoundException(_localizer);
            }

            if (await _userManager.IsInRoleAsync(user, RolesConstant.SuperAdmin))
            {
                return await Result<string>.FailAsync(_localizer["Not Allowed."]);
            }

            foreach (var userRole in request.UserRoles)
            {
                if (await _roleManager.FindByNameAsync(userRole.RoleName) != null)
                {
                    if (userRole.Selected)
                    {
                        if (!await _userManager.IsInRoleAsync(user, userRole.RoleName))
                        {
                            await _userManager.AddToRoleAsync(user, userRole.RoleName);
                        }
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(user, userRole.RoleName);
                    }
                }
            }

            return await Result<string>.SuccessAsync(userId, string.Format(_localizer["User Roles Updated Successfull."]));
        }

        public async Task<Result<string>> ExportUsersAsync(string searchString = "")
        {
            var userFilterSpec = new UserFilterSpecification(searchString);
            var userList = await _userManager.Users
                .AsNoTracking()
                .AsQueryable()
                .Specify(userFilterSpec)
                .OrderByDescending(a => a.CreatedOn)
                .ToListAsync();
            if (userList == null)
            {
                throw new UserListEmptyException(_localizer);
            }

            var user = await _userManager.FindByIdAsync(_currentUser.GetUserId().ToString());
            string result = await _excelService.ExportAsync(userList, mappers: new Dictionary<string, Func<InmoUser, object>>
                {
                    { _localizer["FirstName"], item => item.FirstName },
                    { _localizer["LastName"], item => item.LastName },
                    { _localizer["UserName"], item => item.UserName },
                    { _localizer["Email"], item => item.Email },
                    { _localizer["EmailConfirmed"], item => item.EmailConfirmed },
                    { _localizer["PhoneNumber"], item => item.PhoneNumber },
                    { _localizer["PhoneNumberConfirmed"], item => item.PhoneNumberConfirmed },
                    { _localizer["IsActive"], item => item.IsActive ? "Yes" : "No" },
                    { _localizer["CreatedOn"], item => item.CreatedOn.ToString("G", CultureInfo.CurrentCulture) }
                }, sheetName: _localizer["Users"]);

            if (user != null)
            {
                await _eventLog.LogCustomEventAsync(new() { Event = "Generate Excel File", Description = $"Exported User To Excel for {user.Email}.", Email = user.Email, UserId = _currentUser.GetUserId() });
            }

            return await Result<string>.SuccessAsync(data: result);
        }

        public async Task<int> GetCountAsync()
        {
            return await _userManager.Users.CountAsync();
        }
    }
}