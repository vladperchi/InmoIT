// --------------------------------------------------------------------------------------------------
// <copyright file="EventLoggerProfile.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using AutoMapper;
using InmoIT.Shared.Core.Entities;
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Dtos.Identity.Logging;

namespace InmoIT.Shared.Infrastructure.Mappings
{
    public class EventLoggerProfile : Profile
    {
        public EventLoggerProfile()
        {
            CreateMap<PaginatedLogFilter, GetAllLogsRequest>()
                .ForMember(dest => dest.OrderBy, opt => opt.ConvertUsing<string>(new OrderByConverter()));
            CreateMap<LogRequest, EventLog>().ReverseMap();
        }
    }
}