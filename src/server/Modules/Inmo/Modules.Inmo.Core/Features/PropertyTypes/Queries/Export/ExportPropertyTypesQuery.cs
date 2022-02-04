// --------------------------------------------------------------------------------------------------
// <copyright file="ExportPropertyTypesQuery.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InmoIT.Modules.Inmo.Core.Abstractions;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Modules.Inmo.Core.Exceptions;
using InmoIT.Modules.Inmo.Core.Features.Owners.Queries.Export;
using InmoIT.Modules.Inmo.Core.Specifications;
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Queries.Export
{
    public class ExportPropertyTypesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportPropertyTypesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportPropertyTypesQueryHandler : IRequestHandler<ExportPropertyTypesQuery, Result<string>>
    {
        private readonly IInmoDbContext _context;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportPropertyTypesQueryHandler> _localizer;

        public ExportPropertyTypesQueryHandler(
            IExcelService excelService,
            IInmoDbContext context,
            IStringLocalizer<ExportPropertyTypesQueryHandler> localizer)
        {
            _context = context;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportPropertyTypesQuery request, CancellationToken cancellationToken)
        {
            var filterSpec = new PropertyTypeFilterSpecification(request.SearchString);
            var data = await _context.PropertyTypes
                .AsNoTracking()
                .AsQueryable()
                .Specify(filterSpec)
                .ToListAsync(cancellationToken);
            if (data == null)
            {
                throw new OwnerListEmptyException(_localizer);
            }

            string result = await _excelService.ExportAsync(data, mappers: new Dictionary<string, Func<PropertyType, object>>
            {
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description },
                { _localizer["Active"], item => item.IsActive ? "Yes" : "No" }
            }, sheetName: _localizer["Property Types"]);

            return await Result<string>.SuccessAsync(data: result);
        }
    }
}