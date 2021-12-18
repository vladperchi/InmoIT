// --------------------------------------------------------------------------------------------------
// <copyright file="RoleClaimProfile.cs" company="InmoIT">
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
    public class RoleClaimProfile : Profile
    {
        public RoleClaimProfile()
        {
            CreateMap<RoleClaimResponse, InmoRoleClaim>()
                .ForMember(nameof(InmoRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(InmoRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();

            CreateMap<RoleClaimRequest, InmoRoleClaim>()
                .ForMember(nameof(InmoRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(InmoRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();

            CreateMap<RoleClaimModel, RoleClaimRequest>();
            CreateMap<RoleClaimModel, InmoRoleClaim>().ReverseMap();
            CreateMap<RoleClaimModel, RoleClaimResponse>().ReverseMap();
        }
    }
}