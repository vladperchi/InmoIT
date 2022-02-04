// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyImageProfile.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using AutoMapper;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Dtos.Inmo.Images;
using InmoIT.Modules.Inmo.Core.Features.Images.Commands;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Features.Images.Queries;

namespace InmoIT.Modules.Inmo.Core.Mappings
{
    public class PropertyImageProfile : Profile
    {
        public PropertyImageProfile()
        {
            CreateMap<AddImageCommand, PropertyImage>().ReverseMap();
            CreateMap<GetByIdCacheableFilter<Guid, PropertyImage>, GetImageByPropertyIdQuery>();
            CreateMap<UpdateImageCommand, PropertyImage>().ReverseMap();
            CreateMap<GetAllPropertyImagesResponse, PropertyImage>().ReverseMap();
            CreateMap<GetPropertyImageByPropertyIdResponse, PropertyImage>().ReverseMap();
            CreateMap<PaginatedPropertyImageFilter, GetAllImagesQuery>()
                .ForMember(dest => dest.OrderBy, opt => opt.ConvertUsing<string>(new OrderByConverter()));
        }
    }
}