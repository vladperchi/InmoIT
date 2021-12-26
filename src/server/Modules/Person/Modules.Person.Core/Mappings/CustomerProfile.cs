// --------------------------------------------------------------------------------------------------
// <copyright file="CustomerProfile.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using AutoMapper;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Modules.Person.Core.Features.Customers.Commands;
using InmoIT.Modules.Person.Core.Features.Customers.Queries;
using InmoIT.Shared.Core.Features.Filters;
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Dtos.Customers;

namespace InmoIT.Modules.Person.Core.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<RegisterCustomerCommand, Customer>().ReverseMap();
            CreateMap<UpdateCustomerCommand, Customer>().ReverseMap();
            CreateMap<GetByIdCacheableFilter<Guid, Customer>, GetCustomerByIdQuery>();
            CreateMap<GetCustomerByIdResponse, Customer>().ReverseMap();
            CreateMap<GetAllCustomersResponse, Customer>().ReverseMap();
            CreateMap<PaginatedCustomerFilter, GetAllCustomersQuery>()
                .ForMember(dest => dest.OrderBy, opt => opt.ConvertUsing<string>(new OrderByConverter()));
        }
    }
}