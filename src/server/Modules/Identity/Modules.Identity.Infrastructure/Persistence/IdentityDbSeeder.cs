// --------------------------------------------------------------------------------------------------
// <copyright file="IdentityDbSeeder.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

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
        private readonly IIdentityDbContext _db;
        private readonly UserManager<InmoUser> _userManager;
        private readonly IStringLocalizer<IdentityDbSeeder> _localizer;
        private readonly RoleManager<InmoRole> _roleManager;

        public IdentityDbSeeder(
            ILogger<IdentityDbSeeder> logger,
            IIdentityDbContext db,
            RoleManager<InmoRole> roleManager,
            UserManager<InmoUser> userManager,
            IStringLocalizer<IdentityDbSeeder> localizer)
        {
            _logger = logger;
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            _localizer = localizer;
        }

        public void Initialize()
        {
            AddDefaultRoles();
            AddSuperAdmin();
            AddAdmin();
            AddManager();
            AddStaff();
            AddBasic();
            _db.SaveChanges();
        }

        private void AddDefaultRoles()
        {
            Task.Run(async () =>
            {
                var roleList = new List<string> { RolesConstant.SuperAdmin, RolesConstant.Admin,  RolesConstant.Manager, RolesConstant.Staff, RolesConstant.Basic };
                foreach (string roleName in roleList)
                {
                    var role = new InmoRole(roleName);
                    var roleInDb = await _roleManager.FindByNameAsync(roleName);
                    if (roleInDb == null)
                    {
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
                var superAdminRole = new InmoRole(RolesConstant.SuperAdmin);
                var superAdminRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.SuperAdmin);
                if (superAdminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(superAdminRole);
                    superAdminRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.SuperAdmin);
                }

                var superUser = new InmoUser
                {
                    FirstName = "Vlaperchi",
                    LastName = "Won",
                    Email = "superadmin@inmoit.com",
                    UserName = "superadmin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true
                };
                var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
                if (superUserInDb == null)
                {
                    await _userManager.CreateAsync(superUser, UserConstant.SuperAdminPassword);
                    var result = await _userManager.AddToRoleAsync(superUser, RolesConstant.SuperAdmin);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(_localizer["Seeded Default SuperAdmin User."]);
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
                var adminRole = new InmoRole(RolesConstant.Admin);
                var adminRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.Admin);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    adminRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.Admin);
                }

                var adminUser = new InmoUser
                {
                    FirstName = "Camilo",
                    LastName = "Soto",
                    Email = "admin@inmoit.com",
                    UserName = "admin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true
                };

                var adminUserInDb = await _userManager.FindByEmailAsync(adminUser.Email);
                if (adminUserInDb == null)
                {
                    await _userManager.CreateAsync(adminUser, UserConstant.AdminPassword);
                    var result = await _userManager.AddToRoleAsync(adminUser, RolesConstant.Admin);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(_localizer["Seeded Default Admin User."]);
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

        private void AddManager()
        {
            Task.Run(async () =>
            {
                var managerRole = new InmoRole(RolesConstant.Manager);
                var managerRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.Manager);
                if (managerRoleInDb == null)
                {
                    await _roleManager.CreateAsync(managerRole);
                    managerRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.Manager);
                }

                var managerUser = new InmoUser
                {
                    FirstName = "Alejandro",
                    LastName = "Dobarganes",
                    Email = "manager@inmoit.com",
                    UserName = "manager",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true
                };
                var managerUserInDb = await _userManager.FindByEmailAsync(managerUser.Email);
                if (managerUserInDb == null)
                {
                    await _userManager.CreateAsync(managerUser, UserConstant.ManagerPassword);
                    var result = await _userManager.AddToRoleAsync(managerUser, RolesConstant.Manager);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(_localizer["Seeded Default Manager User."]);
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
                var staffRole = new InmoRole(RolesConstant.Staff);
                var staffRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.Staff);
                if (staffRoleInDb == null)
                {
                    await _roleManager.CreateAsync(staffRole);
                    staffRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.Staff);
                }

                var staffUser = new InmoUser
                {
                    FirstName = "Juan David",
                    LastName = "Vanegas",
                    Email = "staff@inmoit.com",
                    UserName = "staff",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true
                };
                var staffUserInDb = await _userManager.FindByEmailAsync(staffUser.Email);
                if (staffUserInDb == null)
                {
                    await _userManager.CreateAsync(staffUser, UserConstant.StaffPassword);
                    var result = await _userManager.AddToRoleAsync(staffUser, RolesConstant.Staff);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(_localizer["Seeded Default Staff User."]);
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

        private void AddBasic()
        {
            Task.Run(async () =>
            {
                var basicRole = new InmoRole(RolesConstant.Basic);
                var basicRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.Basic);
                if (basicRoleInDb == null)
                {
                    await _roleManager.CreateAsync(basicRole);
                    basicRoleInDb = await _roleManager.FindByNameAsync(RolesConstant.Basic);
                }

                var basicUser = new InmoUser
                {
                    FirstName = "Mario Alberto",
                    LastName = "Rodrigues",
                    Email = "basic@inmoit.com",
                    UserName = "basic",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true
                };
                var basicUserInDb = await _userManager.FindByEmailAsync(basicUser.Email);
                if (basicUserInDb == null)
                {
                    await _userManager.CreateAsync(basicUser, UserConstant.DefaultPassword);
                    var result = await _userManager.AddToRoleAsync(basicUser, RolesConstant.Basic);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(_localizer["Seeded Default User."]);
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