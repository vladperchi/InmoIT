// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyTypeProfile.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using AutoMapper;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Commands;
using InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Queries;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Dtos.Inmo.PropertyTypes;

namespace InmoIT.Modules.Inmo.Core.Mappings
{
    public class PropertyTypeProfile : Profile
    {
        public PropertyTypeProfile()
        {
            CreateMap<CreatePropertyTypeCommand, PropertyType>().ReverseMap();
            CreateMap<GetByIdCacheableFilter<Guid, PropertyType>, GetPropertyTypeByIdQuery>();
            CreateMap<UpdatePropertyTypeCommand, PropertyType>().ReverseMap();
            CreateMap<GetAllPropertyTypesResponse, PropertyType>().ReverseMap();
            CreateMap<GetPropertyTypeByIdResponse, PropertyType>().ReverseMap();
            CreateMap<PaginatedPropertyTypeFilter, GetAllPropertyTypesQuery>()
                .ForMember(dest => dest.OrderBy, opt => opt.ConvertUsing<string>(new OrderByConverter()));
        }
    }
}