﻿// --------------------------------------------------------------------------------------------------
// <copyright file="IdentityDbSeeder.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Modules.Identity.Core.Constants;
using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Modules.Identity.Core.Helpers;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace InmoIT.Modules.Identity.Infrastructure.Persistence
{
    internal class IdentityDbSeeder : IDbSeeder
    {
        private readonly ILogger<IdentityDbSeeder> _logger;
        private readonly IIdentityDbContext _context;
        private readonly UserManager<InmoUser> _userManager;
        private readonly IStringLocalizer<IdentityDbSeeder> _localizer;
        private readonly RoleManager<InmoRole> _roleManager;

        public IdentityDbSeeder(
            ILogger<IdentityDbSeeder> logger,
            IIdentityDbContext context,
            RoleManager<InmoRole> roleManager,
            UserManager<InmoUser> userManager,
            IStringLocalizer<IdentityDbSeeder> localizer)
        {
            _logger = logger;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _localizer = localizer;
        }

        public void Initialize()
        {
            try
            {
                AddDefaultRoles();
                AddSuperAdmin();
                AddAdmin();
                AddStaff();
                _context.SaveChanges();
            }
            catch (Exception)
            {
                _logger.LogError(_localizer["An error occurred while seeding data module Identity."]);
            }
        }

        private void AddDefaultRoles()
        {
            Task.Run(async () =>
            {
                var roleList = new List<string> { RolesConstant.SuperAdmin, RolesConstant.Admin, RolesConstant.Manager, RolesConstant.Accountant, RolesConstant.Supervisor, RolesConstant.Seller, RolesConstant.Staff };
                foreach (string roleName in roleList)
                {
                    var role = new InmoRole(roleName);
                    var roleInDb = await _roleManager.FindByNameAsync(roleName);
                    if (roleInDb == null)
                    {
                        role.Description = $"ROLE {roleName.ToUpper()}";
                        await _roleManager.CreateAsync(role);
                        _logger.LogInformation(string.Format(_localizer["Added '{0}' to Roles"], roleName));
                    }
                }
            }).GetAwaiter().GetResult();
        }

        private void AddSuperAdmin()
        {
            Task.Run(async () =>
            {
                var superAdminRole = new InmoRole(RolesConstant.SuperAdmin, _localizer["SuperAdmin role with default permissions"]);
                var superAdminRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.SuperAdmin);
                if (superAdminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(superAdminRole);
                    superAdminRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.SuperAdmin);
                    _logger.LogInformation(_localizer["Seeded SuperAdmin Role."]);
                }

                var superUser = new InmoUser
                {
                    FirstName = "Vlaperchi",
                    LastName = "Won",
                    Email = "vlaperchiwon@inmoit.com",
                    UserName = "superadmin",
                    PhoneNumber = "0012134855189",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true,
                    CreatedOn = DateTime.Now
                };
                var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
                if (superUserInDb == null)
                {
                    await _userManager.CreateAsync(superUser, UserConstant.SuperAdminPassword);
                    var result = await _userManager.AddToRoleAsync(superUser, RolesConstant.SuperAdmin);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(_localizer["Seeded default user with SuperAdmin Role."]);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }

                foreach (string permission in typeof(PermissionsConstant).GetNestedStringValues())
                {
                    await _roleManager.AddPermissionClaimAsync(superAdminRoleInDb, permission);
                }
            }).GetAwaiter().GetResult();
        }

        private void AddAdmin()
        {
            Task.Run(async () =>
            {
                var adminRole = new InmoRole(RolesConstant.Admin, _localizer["Admin role with default permissions"]);
                var adminRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.Admin);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    adminRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.Admin);
                    _logger.LogInformation(_localizer["Seeded Admin Role."]);
                }

                var adminUser = new InmoUser
                {
                    FirstName = "Camilo",
                    LastName = "Soto",
                    Email = "camilosoto@inmoit.com",
                    UserName = "admincamilo",
                    PhoneNumber = "0012134815671",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true,
                    CreatedOn = DateTime.Now
                };

                var adminUserInDb = await _userManager.FindByEmailAsync(adminUser.Email);
                if (adminUserInDb == null)
                {
                    await _userManager.CreateAsync(adminUser, UserConstant.AdminPassword);
                    var result = await _userManager.AddToRoleAsync(adminUser, RolesConstant.Admin);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(_localizer["Seeded default user with Admin Role."]);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }
            }).GetAwaiter().GetResult();
        }

        private void AddStaff()
        {
            Task.Run(async () =>
            {
                var staffRole = new InmoRole(RolesConstant.Staff, _localizer["Staff role with default permissions"]);
                var staffRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.Staff);
                if (staffRoleInDb == null)
                {
                    await _roleManager.CreateAsync(staffRole);
                    staffRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.Staff);
                    _logger.LogInformation(_localizer["Seeded Staff Role."]);
                }

                var staffUser = new InmoUser
                {
                    FirstName = "David",
                    LastName = "Vanegas",
                    Email = "davidvanegas@inmoit.com",
                    UserName = "staffdavid",
                    PhoneNumber = "0012134858927",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true,
                    CreatedOn = DateTime.Now
                };
                var staffUserInDb = await _userManager.FindByEmailAsync(staffUser.Email);
                if (staffUserInDb == null)
                {
                    await _userManager.CreateAsync(staffUser, UserConstant.BasicPassword);
                    var result = await _userManager.AddToRoleAsync(staffUser, RolesConstant.Staff);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(_localizer["Seeded default user with Staff Role."]);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }
            }).GetAwaiter().GetResult();
        }
    }
}