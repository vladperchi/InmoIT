// --------------------------------------------------------------------------------------------------
// <copyright file="CartProfile.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using AutoMapper;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Modules.Person.Core.Features.Carts.Commands;
using InmoIT.Modules.Person.Core.Features.Carts.Queries;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Dtos.Person.Carts;

namespace InmoIT.Modules.Person.Core.Mappings
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CreateCartCommand, Cart>().ReverseMap();
            CreateMap<GetByIdCacheableFilter<Guid, Cart>, GetCartByIdQuery>();
            CreateMap<GetCartByIdResponse, Cart>().ReverseMap();
            CreateMap<GetAllCartsResponse, Cart>().ReverseMap();
            CreateMap<PaginatedCartFilter, GetAllCartsQuery>()
                .ForMember(dest => dest.OrderBy, opt => opt.ConvertUsing<string>(new OrderByConverter()));
        }
    }
}