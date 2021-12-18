// --------------------------------------------------------------------------------------------------
// <copyright file="RoleProfile.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using AutoMapper;
using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Shared.Dtos.Identity.Roles;

namespace InmoIT.Modules.Identity.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, InmoRole>().ReverseMap();
        }
    }
}