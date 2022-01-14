// --------------------------------------------------------------------------------------------------
// <copyright file="ExportPropertiesQuery.cs" company="InmoIT">
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
using InmoIT.Modules.Inmo.Core.Specifications;
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Inmo.Core.Features.Properties.Queries.Export
{
    public class ExportPropertiesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportPropertiesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportPropertiesQueryHandler : IRequestHandler<ExportPropertiesQuery, Result<string>>
    {
        private readonly IInmoDbContext _context;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportPropertiesQueryHandler> _localizer;

        public ExportPropertiesQueryHandler(
            IExcelService excelService,
            IInmoDbContext context,
            IStringLocalizer<ExportPropertiesQueryHandler> localizer)
        {
            _context = context;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportPropertiesQuery request, CancellationToken cancellationToken)
        {
            var propertyFilterSpec = new PropertyFilterSpecification(request.SearchString);
            var propertyList = await _context.Properties
                .Where(x => x.IsActive)
                .AsNoTracking()
                .AsQueryable()
                .Specify(propertyFilterSpec)
                .ToListAsync(cancellationToken);
            if (propertyList == null)
            {
                throw new PropertyListEmptyException(_localizer);
            }

            string result = await _excelService.ExportAsync(propertyList, mappers: new Dictionary<string, Func<Property, object>>
            {
                { _localizer["CodeInternal"], item => item.CodeInternal },
                { _localizer["Name"], item => item.Name },
                { _localizer["Address"], item => item.Address },
                { _localizer["Description"], item => item.Description },
                { _localizer["Year"], item => item.Year },
                { _localizer["Total"], item => item.Tolal }
            }, sheetName: _localizer["Properties"]);

            return await Result<string>.SuccessAsync(data: result);
        }
    }
}