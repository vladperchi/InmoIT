// --------------------------------------------------------------------------------------------------
// <copyright file="ExcelService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Interfaces.Services;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace InmoIT.Shared.Infrastructure.Services
{
    public class ExcelService : IExcelService
    {
        private readonly IStringLocalizer<ExcelService> _localizer;

        public ExcelService(IStringLocalizer<ExcelService> localizer)
        {
            _localizer = localizer;
        }

        public async Task<string> ExportAsync<TData>(
            IEnumerable<TData> data,
            Dictionary<string, Func<TData, object>> mappers,
            string sheetName)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using var p = new ExcelPackage();
            p.Workbook.Properties.Author = "InmoIT";
            p.Workbook.Worksheets.Add(_localizer["Event Logs"]);
            var ws = p.Workbook.Worksheets[0];
            ws.Name = sheetName;
            ws.Cells.Style.Font.Size = 11;
            ws.Cells.Style.Font.Name = "Calibri";

            int colIndex = 1;
            int rowIndex = 1;

            var headers = mappers.Keys.Select(x => x).ToList();

            foreach (string header in headers)
            {
                var cell = ws.Cells[rowIndex, colIndex];

                var fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightBlue);

                var border = cell.Style.Border;
                border.Bottom.Style =
                    border.Top.Style =
                        border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                cell.Value = header;

                colIndex++;
            }

            var dataList = data.ToList();
            foreach (var item in dataList)
            {
                colIndex = 1;
                rowIndex++;

                var result = headers.Select(header => mappers[header](item));

                foreach (object value in result)
                {
                    ws.Cells[rowIndex, colIndex++].Value = value;
                }
            }

            using (ExcelRange autoFilterCells = ws.Cells[1, 1, dataList.Count + 1, headers.Count])
            {
                autoFilterCells.AutoFilter = true;
                autoFilterCells.AutoFitColumns();
            }

            byte[] byteArray = await p.GetAsByteArrayAsync();
            return Convert.ToBase64String(byteArray);
        }
    }
}