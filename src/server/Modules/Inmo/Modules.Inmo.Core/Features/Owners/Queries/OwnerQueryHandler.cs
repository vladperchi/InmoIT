// --------------------------------------------------------------------------------------------------
// <copyright file="OwnerQueryHandler.cs" company="InmoIT">
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
using InmoIT.Modules.Inmo.Core.Abstractions;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Exceptions;
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Core.Mappings.Converters;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.Owners;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using InmoIT.Modules.Inmo.Core.Specifications;

namespace InmoIT.Modules.Inmo.Core.Features.Owners.Queries
{
    internal class OwnerQueryHandler :
       IRequestHandler<GetAllOwnersQuery, PaginatedResult<GetAllOwnersResponse>>,
       IRequestHandler<GetOwnerByIdQuery, Result<GetOwnerByIdResponse>>,
       IRequestHandler<GetOwnerImageQuery, Result<string>>
    {
        private readonly IInmoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<OwnerQueryHandler> _localizer;

        public OwnerQueryHandler(
            IInmoDbContext context,
            IMapper mapper,
            IStringLocalizer<OwnerQueryHandler> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedResult<GetAllOwnersResponse>> Handle(GetAllOwnersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Owner, GetAllOwnersResponse>> expression = e => new GetAllOwnersResponse(e.Id, e.Name, e.SurName, e.Address, e.ImageUrl, e.Birthday, e.Gender, e.Group, e.Email, e.PhoneNumber);
            var sourse = _context.Owners
                .Where(x => x.IsActive)
                .AsNoTracking()
                .AsQueryable();
            string ordering = new OrderByConverter().Convert(request.OrderBy);
            sourse = !string.IsNullOrWhiteSpace(ordering)
                ? sourse.OrderBy(ordering)
                : sourse.OrderBy(o => o.Id);
            var filterSpec = new OwnerFilterSpecification(request.SearchString);
            var data = await sourse
                .Specify(filterSpec)
                .Select(expression)
                .AsNoTracking()
                .ToPaginatedListAsync(request.PageNumber, request.PageSize, _localizer);
            if (data == null)
            {
                throw new OwnerListEmptyException(_localizer);
            }

            return _mapper.Map<PaginatedResult<GetAllOwnersResponse>>(data);
        }

        public async Task<Result<GetOwnerByIdResponse>> Handle(GetOwnerByIdQuery query, CancellationToken cancellationToken)
        {
            var data = await _context.Owners
                .Where(x => x.Id == query.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (data == null)
            {
                throw new OwnerNotFoundException(_localizer);
            }

            var result = _mapper.Map<GetOwnerByIdResponse>(data);
            return await Result<GetOwnerByIdResponse>.SuccessAsync(result);
        }

        public async Task<Result<string>> Handle(GetOwnerImageQuery request, CancellationToken cancellationToken)
        {
            string result = await _context.Owners
                .AsNoTracking()
                .Where(c => c.Id == request.Id)
                .Select(a => a.ImageUrl)
                .FirstOrDefaultAsync(cancellationToken);

            return await Result<string>.SuccessAsync(result);
        }
    }
}