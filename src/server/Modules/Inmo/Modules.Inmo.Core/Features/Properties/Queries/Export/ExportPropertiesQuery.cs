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
using InmoIT.Shared.Core.Integration.Inmo;
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
        private readonly IPropertyService _propertyService;
        private readonly IStringLocalizer<ExportPropertiesQueryHandler> _localizer;

        public ExportPropertiesQueryHandler(
            IExcelService excelService,
            IInmoDbContext context,
            IPropertyService propertyService,
            IStringLocalizer<ExportPropertiesQueryHandler> localizer)
        {
            _context = context;
            _excelService = excelService;
            _propertyService = propertyService;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportPropertiesQuery request, CancellationToken cancellationToken)
        {
            var filterSpec = new PropertyFilterSpecification(request.SearchString);
            var data = await _context.Properties
                .Where(x => x.IsActive)
                .AsNoTracking()
                .AsQueryable()
                .Specify(filterSpec)
                .ToListAsync(cancellationToken);
            if (data == null)
            {
                throw new PropertyListEmptyException(_localizer);
            }

            try
            {
                string result = await _excelService.ExportAsync(data, mappers: new Dictionary<string, Func<Property, object>>
            {
                { _localizer["Owner"], item => item.OwnerName.ToUpper() },
                { _localizer["CodeInternal"], item => item.CodeInternal.ToUpper() },
                { _localizer["Available"], item => item.IsActive ? "Yes" : "No" },
                { _localizer["Type"], item => item.PropertyTypeName },
                { _localizer["Name"], item => item.Name },
                { _localizer["Address"], item => item.Address },
                { _localizer["Description"], item => item.Description },
                { _localizer["Year"], item => item.Year },
                { _localizer["Area"], item => item.SquareMeter },
                { _localizer["Rooms"], item => item.NumberRooms },
                { _localizer["Bathrooms"], item => item.NumberBathrooms },
                { _localizer["Parking"], item => item.HasParking ? "Yes" : "No" },
                { _localizer["Rent Price"], item => item.TotalRent.ToString("0:N2") },
                { _localizer["Sale Price"], item => item.TolalSale.ToString("C") }
            }, sheetName: _localizer["Properties"]);

                return await Result<string>.SuccessAsync(data: result);
            }
            catch (Exception)
            {
                throw new PropertyCustomException(_localizer, null);
            }
        }
    }
}