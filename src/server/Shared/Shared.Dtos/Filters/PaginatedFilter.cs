// --------------------------------------------------------------------------------------------------
// <copyright file="PaginatedFilter.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Dtos.Filters
{
  public class PaginatedFilter : BaseFilter
  {
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public string OrderBy { get; set; }

    public PaginatedFilter()
    {
      PageNumber = 1;
      PageSize = 10;
    }

    public PaginatedFilter(int pageNumber, int pageSize)
    {
      PageNumber = pageNumber < 1 ? 1 : pageNumber;
      PageSize = pageSize > 10 ? 10 : pageSize;
    }
  }
}