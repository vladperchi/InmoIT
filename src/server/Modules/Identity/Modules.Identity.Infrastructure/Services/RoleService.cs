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
using System.Net;
using AutoMapper;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Modules.Identity.Core.Exceptions;
using InmoIT.Modules.Identity.Core.Features.Roles.Events;
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

        private static List<string> DefaultRoles()
        {
            return typeof(RolesConstant).GetAllConstantValues<string>();
        }

        public async Task<Result<string>> DeleteAsync(string id)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole == null)
            {
                throw new IdentityException("Role Not Found", statusCode: HttpStatusCode.NotFound);
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
                await _roleManager.DeleteAsync(existingRole);
                return await Result<string>.SuccessAsync(existingRole.Id, string.Format(_localizer["Role {0} Deleted."], existingRole.Name));
            }
            else
            {
                return await Result<string>.FailAsync(string.Format(_localizer["Not allowed to delete {0} Role as it is being used."], existingRole.Name));
            }
        }

        public async Task<Result<List<RoleResponse>>> GetAllAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var result = _mapper.Map<List<RoleResponse>>(roles);
            return await Result<List<RoleResponse>>.SuccessAsync(result);
        }

        public async Task<Result<RoleResponse>> GetByIdAsync(string id)
        {
            var roles = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == id);
            var result = _mapper.Map<RoleResponse>(roles);
            return await Result<RoleResponse>.SuccessAsync(result);
        }

        public async Task<Result<string>> SaveAsync(RoleRequest request)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var existingRole = await _roleManager.FindByNameAsync(request.Name);
                if (existingRole != null)
                {
                    throw new IdentityException(_localizer["Similar Role exists."], statusCode: HttpStatusCode.BadRequest);
                }

                var newRole = new InmoRole(request.Name, request.Description);
                var response = await _roleManager.CreateAsync(newRole);
                newRole.AddDomainEvent(new RoleAddedEvent(newRole));
                await _context.SaveChangesAsync();
                if (response.Succeeded)
                {
                    return await Result<string>.SuccessAsync(newRole.Id, string.Format(_localizer["Role {0} Created."], request.Name));
                }
                else
                {
                    return await Result<string>.FailAsync(response.Errors.Select(e => _localizer[e.Description].ToString()).ToList());
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
                await _roleManager.UpdateAsync(existingRole);
                return await Result<string>.SuccessAsync(existingRole.Id, string.Format(_localizer["Role {0} Updated."], existingRole.Name));
            }
        }

        public async Task<int> GetCountAsync()
        {
            return await _roleManager.Roles.CountAsync();
        }
    }
}