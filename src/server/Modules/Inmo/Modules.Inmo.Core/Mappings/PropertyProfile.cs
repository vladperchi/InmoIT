// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyProfile.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using AutoMapper;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Features.Properties.Commands;
using InmoIT.Modules.Inmo.Core.Features.Properties.Queries;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Dtos.Inmo.Properties;

namespace InmoIT.Modules.Inmo.Core.Mappings
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<RegisterPropertyCommand, Property>().ReverseMap();
            CreateMap<UpdatePropertyCommand, Property>().ReverseMap();
            CreateMap<GetByIdCacheableFilter<Guid, Property>, GetPropertyByIdQuery>();
            CreateMap<GetAllPropertiesResponse, Property>().ReverseMap();
            CreateMap<GetPropertyByIdResponse, Property>().ReverseMap();
            CreateMap<Property, GetAllPropertiesResponse>()
                .ForMember(x => x.OwnerName, o => o.MapFrom(s => $"{s.Owner.Name} {s.Owner.SurName}"))
                .ForMember(x => x.TypeName, o => o.MapFrom(s => $"{s.PropertyType.Name}"));
            CreateMap<PaginatedPropertyFilter, GetAllPropertiesQuery>()
                .ForMember(dest => dest.OrderBy, opt => opt.ConvertUsing<string>(new OrderByConverter()));
        }
    }
}