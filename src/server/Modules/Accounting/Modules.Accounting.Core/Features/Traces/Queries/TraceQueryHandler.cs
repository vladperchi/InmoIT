// --------------------------------------------------------------------------------------------------
// <copyright file="TraceQueryHandler.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InmoIT.Modules.Accounting.Core.Abstractions;
using InmoIT.Modules.Accounting.Core.Entities;
using InmoIT.Modules.Accounting.Core.Exceptions;
using InmoIT.Modules.Accounting.Core.Specifications;
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Accounting.Traces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Accounting.Core.Features.Traces.Queries
{
    internal class TraceQueryHandler :
        IRequestHandler<GetAllTracesQuery, PaginatedResult<GetAllTracesResponse>>
    {
        private readonly IAccountingDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<TraceQueryHandler> _localizer;

        public TraceQueryHandler(
            IAccountingDbContext context,
            IMapper mapper,
            IStringLocalizer<TraceQueryHandler> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedResult<GetAllTracesResponse>> Handle(GetAllTracesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<PropertyTrace, GetAllTracesResponse>> expression = e => new GetAllTracesResponse(e.Id, e.Code, e.Name, e.Value, e.Tax, e.Type.ToString(), e.CreatedOn, e.UpdatedOn, e.PropertyId);
            var sourse = _context.PropertyTraces
            .AsNoTracking()
            .AsQueryable();
            string ordering = new OrderByConverter().Convert(request.OrderBy);
            sourse = !string.IsNullOrWhiteSpace(ordering)
                ? sourse.OrderBy(ordering)
                : sourse.OrderBy(a => a.Id);
            if (request.PropertyId != null && !request.PropertyId.Equals(Guid.Empty))
            {
                sourse = sourse.Where(x => x.PropertyId.Equals(request.PropertyId));
            }

            var filterSpec = new TraceFilterSpecification(request.SearchString);
            var data = await sourse
                .Specify(filterSpec)
                .Select(expression)
                .AsNoTracking()
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            if (data == null)
            {
                throw new TraceListEmptyException(_localizer);
            }

            return _mapper.Map<PaginatedResult<GetAllTracesResponse>>(data);
        }
    }
}