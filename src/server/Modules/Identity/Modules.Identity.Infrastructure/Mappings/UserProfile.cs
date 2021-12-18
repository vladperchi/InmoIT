// --------------------------------------------------------------------------------------------------
// <copyright file="UserProfile.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using AutoMapper;
using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Shared.Dtos.Identity.Users;

namespace InmoIT.Modules.Identity.Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, InmoUser>().ReverseMap();
            CreateMap<UpdateUserRequest, InmoUser>().ReverseMap();
        }
    }
}