// --------------------------------------------------------------------------------------------------
// <copyright file="RoleService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Modules.Identity.Core.Exceptions;
using InmoIT.Modules.Identity.Core.Features.Roles.Events;
using InmoIT.Modules.Identity.Infrastructure.Extensions;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Identity.Roles;
using InmoIT.Shared.Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Identity.Infrastructure.Services
{
    internal class RoleService : IRoleService
    {
        private readonly RoleManager<InmoRole> _roleManager;
        private readonly UserManager<InmoUser> _userManager;
        private readonly IIdentityDbContext _context;
        private readonly IStringLocalizer<RoleService> _localizer;
        private readonly IMapper _mapper;

        public RoleService(
            RoleManager<InmoRole> roleManager,
            UserManager<InmoUser> userManager,
            IMapper mapper,
            IIdentityDbContext context,
            IStringLocalizer<RoleService> localizer)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
            _localizer = localizer;
        }

        public async Task<Result<List<RoleResponse>>> GetAllAsync()
        {
            var data = await _roleManager.Roles.ToListAsync();
            if(data == null)
            {
                throw new RolListEmptyException(_localizer);
            }

            var result = _mapper.Map<List<RoleResponse>>(data);
            return await Result<List<RoleResponse>>.SuccessAsync(result);
        }

        public async Task<Result<RoleResponse>> GetByIdAsync(string id)
        {
            var data = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                throw new RoleNotFoundException(_localizer);
            }

            var result = _mapper.Map<RoleResponse>(data);
            return await Result<RoleResponse>.SuccessAsync(result);
        }

        public async Task<Result<string>> AddOrUpdateAsync(RoleRequest request)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var existingRole = await _roleManager.FindByNameAsync(request.Name);
                if (existingRole != null)
                {
                    throw new RoleAlreadyExistsException(_localizer);
                }

                var newRole = new InmoRole(request.Name, request.Description);
                var result = await _roleManager.CreateAsync(newRole);
                newRole.AddDomainEvent(new RoleAddedEvent(newRole));
                await _context.SaveChangesAsync();
                if (result.Succeeded)
                {
                    return await Result<string>.SuccessAsync(newRole.Id, string.Format(_localizer["Role {0} Created."], request.Name));
                }
                else
                {
                    return await Result<string>.FailAsync(result.GetErrorMessages(_localizer));
                    throw new IdentityException(_localizer["An error occurred while added Rol"], result.GetErrorMessages(_localizer));
                }
            }
            else
            {
                var existingRole = await _roleManager.FindByIdAsync(request.Id);
                if (existingRole == null)
                {
                    return await Result<string>.FailAsync(_localizer["Role does not exist."]);
                }

                if (DefaultRoles().Contains(existingRole.Name))
                {
                    return await Result<string>.SuccessAsync(string.Format(_localizer["Not allowed to modify {0} Role."], existingRole.Name));
                }

                existingRole.Name = request.Name;
                existingRole.NormalizedName = request.Name.ToUpper();
                existingRole.Description = request.Description;
                existingRole.AddDomainEvent(new RoleUpdatedEvent(existingRole));
                var result = await _roleManager.UpdateAsync(existingRole);
                if (result.Succeeded)
                {
                    return await Result<string>.SuccessAsync(existingRole.Id, string.Format(_localizer["Role {0} Updated."], existingRole.Name));
                }
                else
                {
                    return await Result<string>.FailAsync(result.GetErrorMessages(_localizer));
                    throw new IdentityException(_localizer["An error occurred while updated Rol"], result.GetErrorMessages(_localizer));
                }
            }
        }

        public async Task<Result<string>> DeleteAsync(string id)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole == null)
            {
                throw new RoleNotFoundException(_localizer);
            }

            if (DefaultRoles().Contains(existingRole.Name))
            {
                return await Result<string>.FailAsync(string.Format(_localizer["Not allowed to delete {0} Role."], existingRole.Name));
            }

            bool roleIsNotUsed = true;
            var allUsers = await _userManager.Users.ToListAsync();
            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, existingRole.Name))
                {
                    roleIsNotUsed = false;
                }
            }

            if (roleIsNotUsed)
            {
                existingRole.AddDomainEvent(new RoleDeletedEvent(id));
                var result = await _roleManager.DeleteAsync(existingRole);
                if (result.Succeeded)
                {
                    return await Result<string>.SuccessAsync(existingRole.Id, string.Format(_localizer["Role {0} Deleted."], existingRole.Name));
                }
                else
                {
                    return await Result<string>.FailAsync(result.GetErrorMessages(_localizer));
                    throw new IdentityException(_localizer["An error occurred while deleted Rol"], result.GetErrorMessages(_localizer));
                }
            }
            else
            {
                return await Result<string>.FailAsync(string.Format(_localizer["Not allowed to delete {0} Role as it is being used."], existingRole.Name));
            }
        }

        private static List<string> DefaultRoles()
        {
            return typeof(RolesConstant).GetAllConstantValues<string>();
        }

        public async Task<int> GetCountAsync() => await _roleManager.Roles.CountAsync();
    }
}