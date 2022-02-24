// --------------------------------------------------------------------------------------------------
// <copyright file="ExportOwnersQuery.cs" company="InmoIT">
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

namespace InmoIT.Modules.Inmo.Core.Features.Owners.Queries.Export
{
    public class ExportOwnersQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportOwnersQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportOwnersQueryHandler : IRequestHandler<ExportOwnersQuery, Result<string>>
    {
        private readonly IInmoDbContext _context;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportOwnersQueryHandler> _localizer;

        public ExportOwnersQueryHandler(
            IExcelService excelService,
            IInmoDbContext context,
            IStringLocalizer<ExportOwnersQueryHandler> localizer)
        {
            _context = context;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportOwnersQuery request, CancellationToken cancellationToken)
        {
            var filterSpec = new OwnerFilterSpecification(request.SearchString);
            var data = await _context.Owners.AsNoTracking().AsQueryable().Specify(filterSpec).ToListAsync(cancellationToken);
            _ = data ?? throw new OwnerListEmptyException(_localizer);
            string result = await _excelService.ExportAsync(data, mappers: new Dictionary<string, Func<Owner, object>>
            {
                { _localizer["Name"], item => item.FullName },
                { _localizer["Active"], item => item.IsActive ? "Yes" : "No" },
                { _localizer["PhoneNumber"], item => item.PhoneNumber },
                { _localizer["Email"], item => item.Email },
                { _localizer["Address"], item => item.Address },
                { _localizer["Birthday"], item => item.Birthday.ToString("G", CultureInfo.CurrentCulture) },
                { _localizer["Gender"], item => item.Gender },
                { _localizer["Group"], item => item.Group }
            }, sheetName: _localizer["Owners"]);
            return await Result<string>.SuccessAsync(data: result);
        }
    }
}