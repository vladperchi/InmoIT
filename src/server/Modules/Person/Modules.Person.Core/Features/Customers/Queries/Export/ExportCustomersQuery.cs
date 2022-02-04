// --------------------------------------------------------------------------------------------------
// <copyright file="ExportCustomersQuery.cs" company="InmoIT">
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
using InmoIT.Modules.Person.Core.Abstractions;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Modules.Person.Core.Exceptions;
using InmoIT.Modules.Person.Core.Specifications;
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Features.Customers.Queries.Export
{
    public class ExportCustomersQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportCustomersQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportCustomersQueryHandler : IRequestHandler<ExportCustomersQuery, Result<string>>
    {
        private readonly ICustomerDbContext _context;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportCustomersQueryHandler> _localizer;

        public ExportCustomersQueryHandler(
            IExcelService excelService,
            ICustomerDbContext context,
            IStringLocalizer<ExportCustomersQueryHandler> localizer)
        {
            _context = context;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportCustomersQuery request, CancellationToken cancellationToken)
        {
            var filterSpec = new CustomerFilterSpecification(request.SearchString);
            var data = await _context.Customers
                .AsNoTracking()
                .AsQueryable()
                .Specify(filterSpec)
                .ToListAsync(cancellationToken);
            if (data == null)
            {
                throw new CustomerListEmptyException(_localizer);
            }

            string result = await _excelService.ExportAsync(data, mappers: new Dictionary<string, Func<Customer, object>>
            {
                { _localizer["Name"], item => item.FullName },
                { _localizer["Active"], item => item.IsActive ? "Yes" : "No" },
                { _localizer["PhoneNumber"], item => item.PhoneNumber },
                { _localizer["Email"], item => item.Email },
                { _localizer["Birthday"], item => item.Birthday.ToString("G", CultureInfo.CurrentCulture) },
                { _localizer["Gender"], item => item.Gender },
                { _localizer["Group"], item => item.Group }
            }, sheetName: _localizer["Customers"]);

            return await Result<string>.SuccessAsync(data: result);
        }
    }
}