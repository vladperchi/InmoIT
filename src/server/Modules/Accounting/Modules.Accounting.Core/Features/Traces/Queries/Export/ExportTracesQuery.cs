// --------------------------------------------------------------------------------------------------
// <copyright file="ExportTracesQuery.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InmoIT.Modules.Accounting.Core.Abstractions;
using InmoIT.Modules.Accounting.Core.Entities;
using InmoIT.Modules.Accounting.Core.Exceptions;
using InmoIT.Modules.Accounting.Core.Specifications;
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Accounting.Core.Features.Traces.Queries.Export
{
    public class ExportTracesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportTracesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportTracesQueryHandler : IRequestHandler<ExportTracesQuery, Result<string>>
    {
        private readonly IAccountingDbContext _context;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportTracesQueryHandler> _localizer;

        public ExportTracesQueryHandler(
            IExcelService excelService,
            IAccountingDbContext context,
            IStringLocalizer<ExportTracesQueryHandler> localizer)
        {
            _context = context;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportTracesQuery request, CancellationToken cancellationToken)
        {
            var filterSpec = new TraceFilterSpecification(request.SearchString);
            var data = await _context.PropertyTraces
                .AsNoTracking()
                .AsQueryable()
                .Specify(filterSpec)
                .OrderByDescending(a => a.CreatedOn)
                .ToListAsync(cancellationToken);
            if (data == null)
            {
                throw new TraceListEmptyException(_localizer);
            }

            string result = await _excelService.ExportAsync(data, mappers: new Dictionary<string, Func<PropertyTrace, object>>
                {
                    { _localizer["Code Internal"], item => item.Code.ToUpper() },
                    { _localizer["Name"], item => item.Name },
                    { _localizer["Price"], item => item.Tolal.ToString("C") },
                    { _localizer["Operation"], item => item.TransactionType == 0 ? "Rent" : "Sale" },
                    { _localizer["CreatedOn"], item => item.CreatedOn.ToString("G", CultureInfo.CurrentCulture) },
                    { _localizer["UpdatedOn"], item => item.UpdatedOn.ToString("G", CultureInfo.CurrentCulture) }
                }, sheetName: _localizer["Traces"]);

            return await Result<string>.SuccessAsync(data: result);
        }
    }
}