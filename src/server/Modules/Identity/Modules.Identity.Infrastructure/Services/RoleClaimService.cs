// --------------------------------------------------------------------------------------------------
// <copyright file="RoleClaimService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Modules.Identity.Core.Exceptions;
using InmoIT.Modules.Identity.Core.Features.RoleClaims.Events;
using InmoIT.Modules.Identity.Core.Helpers;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Interfaces.Services.Identity;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Identity.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Identity.Infrastructure.Services
{
    public class RoleClaimService : IRoleClaimService
    {
        private readonly RoleManager<InmoRole> _roleManager;
        private readonly UserManager<InmoUser> _userManager;
        private readonly ICurrentUser _currentUser;
        private readonly IStringLocalizer<RoleClaimService> _localizer;
        private readonly IMapper _mapper;
        private readonly IIdentityDbContext _context;

        public RoleClaimService(
            RoleManager<InmoRole> roleManager,
            UserManager<InmoUser> userManager,
            ICurrentUser currentUserService,
            IStringLocalizer<RoleClaimService> localizer,
            IMapper mapper,
            IIdentityDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _currentUser = currentUserService;
            _localizer = localizer;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<List<RoleClaimResponse>>> GetAllAsync()
        {
            var data = await _context.RoleClaims.AsNoTracking().ToListAsync();
            if (data == null)
            {
                throw new RolClaimListEmptyException(_localizer);
            }

            var result = _mapper.Map<List<RoleClaimResponse>>(data);
            return await Result<List<RoleClaimResponse>>.SuccessAsync(result);
        }

        public async Task<Result<RoleClaimResponse>> GetByIdAsync(int id)
        {
            var data = await _context.RoleClaims.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                throw new RoleClaimNotFoundException(_localizer);
            }

            var result = _mapper.Map<RoleClaimResponse>(data);
            return await Result<RoleClaimResponse>.SuccessAsync(result);
        }

        public async Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId)
        {
            var data = await _context.RoleClaims.AsNoTracking().Include(x => x.Role).Where(x => x.RoleId == roleId).ToListAsync();
            if (data == null)
            {
                throw new RolClaimListEmptyException(_localizer);
            }

            var result = _mapper.Map<List<RoleClaimResponse>>(data);
            return await Result<List<RoleClaimResponse>>.SuccessAsync(result);
        }

        public async Task<Result<string>> SaveAsync(RoleClaimRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RoleId))
            {
                return await Result<string>.FailAsync(_localizer["Role is required."]);
            }

            if (request.Id == 0)
            {
                var existingRoleClaim = await _context.RoleClaims.SingleOrDefaultAsync(x => x.RoleId == request.RoleId && x.ClaimType == request.Type && x.ClaimValue == request.Value);
                if (existingRoleClaim != null)
                {
                    return await Result<string>.FailAsync(_localizer["Similar Role Claim exists."]);
                }

                var roleClaim = _mapper.Map<InmoRoleClaim>(request);
                await _context.RoleClaims.AddAsync(roleClaim);
                roleClaim.AddDomainEvent(new RoleClaimAddedEvent(roleClaim));
                await _context.SaveChangesAsync();
                return await Result<string>.SuccessAsync(string.Format(_localizer["Role Claim {0} created."], request.Value));
            }
            else
            {
                var existingRoleClaim =
                    await _context.RoleClaims.Include(x => x.Role).SingleOrDefaultAsync(x => x.Id == request.Id);
                if (existingRoleClaim == null)
                {
                    return await Result<string>.FailAsync(_localizer["Role Claim does not exist."]);
                }
                else
                {
                    existingRoleClaim.ClaimType = request.Type;
                    existingRoleClaim.ClaimValue = request.Value;
                    existingRoleClaim.Group = request.Group;
                    existingRoleClaim.Description = request.Description;
                    existingRoleClaim.RoleId = request.RoleId;
                    _context.RoleClaims.Update(existingRoleClaim);
                    existingRoleClaim.AddDomainEvent(new RoleClaimUpdatedEvent(existingRoleClaim));
                    await _context.SaveChangesAsync();
                    return await Result<string>.SuccessAsync(string.Format(_localizer["Role Claim {0} for Role {1} updated."], request.Value, existingRoleClaim.Role.Name));
                }
            }
        }

        public async Task<Result<string>> DeleteAsync(int id)
        {
            var existingRoleClaim = await _context.RoleClaims.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id);
            if (existingRoleClaim != null)
            {
                if (existingRoleClaim.Role?.Name == RolesConstant.SuperAdmin)
                {
                    return await Result<string>.FailAsync(string.Format(_localizer["Not allowed to delete Permissions for {0} Role."], existingRoleClaim.Role.Name));
                }

                _context.RoleClaims.Remove(existingRoleClaim);
                existingRoleClaim.AddDomainEvent(new RoleClaimDeletedEvent(id));
                await _context.SaveChangesAsync();
                return await Result<string>.SuccessAsync(string.Format(_localizer["Role Claim {0} for {1} Role deleted."], existingRoleClaim.ClaimValue, existingRoleClaim.Role?.Name));
            }
            else
            {
                return await Result<string>.FailAsync(_localizer["Role Claim does not exist."]);
            }
        }

        public async Task<Result<PermissionResponse>> GetAllPermissionsAsync(string roleId)
        {
            var response = new PermissionResponse
            {
                RoleClaims = new()
            };
            response.RoleClaims.GetAllPermissions();
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                response.RoleId = role.Id;
                response.RoleName = role.Name;
                var allRoleClaims = await GetAllAsync();
                var roleClaimsResult = await GetAllByRoleIdAsync(role.Id);
                if (roleClaimsResult.Succeeded)
                {
                    var roleClaims = roleClaimsResult.Data;
                    var allClaimValues = response.RoleClaims.Select(a => a.Value).ToList();
                    var roleClaimValues = roleClaims.Select(a => a.Value).ToList();
                    var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
                    foreach (var permission in response.RoleClaims)
                    {
                        permission.Id = allRoleClaims.Data?.SingleOrDefault(x => x.RoleId == roleId && x.Type == permission.Type && x.Value == permission.Value)?.Id ?? 0;
                        permission.RoleId = roleId;
                        if (authorizedClaims.Any(a => a == permission.Value))
                        {
                            permission.Selected = true;
                            var roleClaim = roleClaims.SingleOrDefault(a => a.Type == permission.Type && a.Value == permission.Value);
                            if (roleClaim?.Description != null)
                            {
                                permission.Description = roleClaim.Description;
                            }

                            if (roleClaim?.Group != null)
                            {
                                permission.Group = roleClaim.Group;
                            }
                        }
                    }
                }
                else
                {
                    response.RoleClaims = new();
                    return await Result<PermissionResponse>.FailAsync(roleClaimsResult.Messages);
                }
            }
            else
            {
                response.RoleClaims = new();
                return await Result<PermissionResponse>.FailAsync(_localizer["Role does not exist."]);
            }

            return await Result<PermissionResponse>.SuccessAsync(response);
        }

        public async Task<Result<string>> UpdatePermissionsAsync(PermissionRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.RoleId))
                {
                    return await Result<string>.FailAsync(_localizer["Role is required."]);
                }

                if (request.RoleClaims.Any(c => !c.Type.Equals(ClaimConstant.Permission)))
                {
                    return await Result<string>.FailAsync(string.Format(_localizer["All Role Claims Type values should be '{0}'."], ClaimConstant.Permission));
                }

                if (request.RoleClaims.Any(c => !c.RoleId.Equals(request.RoleId)))
                {
                    return await Result<string>.FailAsync(string.Format(_localizer["All Role Claims should contain the same Role Id as in the request."], ClaimConstant.Permission));
                }

                var errors = new List<string>();
                var role = await _roleManager.FindByIdAsync(request.RoleId);
                if (role != null)
                {
                    if (role.Name == RolesConstant.SuperAdmin)
                    {
                        var currentUser = await _userManager.Users.SingleAsync(x => x.Id == _currentUser.GetUserId().ToString());
                        if (!await _userManager.IsInRoleAsync(currentUser, RolesConstant.SuperAdmin))
                        {
                            return await Result<string>.FailAsync(_localizer["Not allowed to modify Permissions for this Role."]);
                        }
                    }

                    var selectedClaims = request.RoleClaims.Where(a => !a.Selected).ToList();
                    if (role.Name == RolesConstant.SuperAdmin)
                    {
                        if (selectedClaims.Any(x => x.Value == PermissionsConstant.Roles.View) ||
                            selectedClaims.Any(x => x.Value == PermissionsConstant.RoleClaims.View) ||
                            selectedClaims.Any(x => x.Value == PermissionsConstant.RoleClaims.Edit))
                        {
                            return await Result<string>.FailAsync(string.Format(
                                _localizer["Not allowed to deselect {0} or {1} or {2} for this Role."],
                                PermissionsConstant.Roles.View,
                                PermissionsConstant.RoleClaims.View,
                                PermissionsConstant.RoleClaims.Edit));
                        }
                    }

                    foreach (var claim in selectedClaims)
                    {
                        if (claim.Id != 0)
                        {
                            var removeResult = await DeleteAsync(claim.Id);
                            if (!removeResult.Succeeded)
                            {
                                errors.AddRange(removeResult.Messages);
                            }
                        }
                    }

                    foreach (var claim in request.RoleClaims.Where(a => a.Selected).ToList())
                    {
                        var saveResult = await SaveAsync(_mapper.Map<RoleClaimRequest>(claim));
                        if (!saveResult.Succeeded)
                        {
                            errors.AddRange(saveResult.Messages);
                        }
                    }

                    if (errors.Count > 0)
                    {
                        return await Result<string>.FailAsync(errors);
                    }

                    return await Result<string>.SuccessAsync(_localizer["Permissions Updated."]);
                }
                else
                {
                    return await Result<string>.FailAsync(_localizer["Role does not exist."]);
                }
            }
            catch (Exception ex)
            {
                return await Result<string>.FailAsync(ex.Message);
            }
        }

        public async Task<int> GetCountAsync() => await _context.RoleClaims.AsNoTracking().CountAsync();
    }
}