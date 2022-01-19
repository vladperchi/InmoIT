// --------------------------------------------------------------------------------------------------
// <copyright file="TraceFilterSpecification.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Accounting.Core.Entities;
using InmoIT.Shared.Core.Interfaces.Specifications;
using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Accounting.Core.Specifications
{
    public class TraceFilterSpecification : Specification<PropertyTrace>
    {
        public TraceFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = x => EF.Functions.Like(x.Code.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.Name.ToLower(), $"%{searchString.ToLower()}%");
            }
            else
            {
                Criteria = _ => true;
            }
        }
    }
}