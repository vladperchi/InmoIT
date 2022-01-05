// --------------------------------------------------------------------------------------------------
// <copyright file="OwnerProfile.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using AutoMapper;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Features.Owners.Commands;
using InmoIT.Modules.Inmo.Core.Features.Owners.Queries;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Dtos.Inmo.Owners;

namespace InmoIT.Modules.Inmo.Core.Mappings
{
    public class OwnerProfile : Profile
    {
        public OwnerProfile()
        {
            CreateMap<RegisterOwnerCommand, Owner>().ReverseMap();
            CreateMap<GetByIdCacheableFilter<Guid, Owner>, GetOwnerByIdQuery>();
            CreateMap<UpdateOwnerCommand, Owner>().ReverseMap();
            CreateMap<GetAllOwnersResponse, Owner>().ReverseMap();
            CreateMap<GetOwnerByIdResponse, Owner>().ReverseMap();
            CreateMap<PaginatedOwnerFilter, GetAllOwnersQuery>()
                .ForMember(dest => dest.OrderBy, opt => opt.ConvertUsing<string>(new OrderByConverter()));
        }
    }
}