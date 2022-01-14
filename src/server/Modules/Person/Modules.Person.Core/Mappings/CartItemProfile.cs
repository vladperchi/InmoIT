// --------------------------------------------------------------------------------------------------
// <copyright file="CartItemProfile.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using AutoMapper;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Modules.Person.Core.Features.CartItems.Commands;
using InmoIT.Modules.Person.Core.Features.CartItems.Queries;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Dtos.Person.CartItems;

namespace InmoIT.Modules.Person.Core.Mappings
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<AddCartItemCommand, CartItem>().ReverseMap();
            CreateMap<UpdateCartItemCommand, CartItem>().ReverseMap();
            CreateMap<GetByIdCacheableFilter<Guid, CartItem>, GetCartItemByIdQuery>();
            CreateMap<GetCartItemByIdResponse, CartItem>().ReverseMap();
            CreateMap<PaginatedCartItemFilter, GetAllCartItemsQuery>()
                .ForMember(dest => dest.OrderBy, opt => opt.ConvertUsing<string>(new OrderByConverter()));
        }
    }
}