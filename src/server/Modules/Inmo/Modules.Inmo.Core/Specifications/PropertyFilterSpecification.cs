// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyFilterSpecification.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Interfaces.Specifications;
using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Inmo.Core.Specifications
{
    public class PropertyFilterSpecification : Specification<Property>
    {
        public PropertyFilterSpecification(string searchString)
        {
            Includes.Add(x => x.Owner);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = x => !string.IsNullOrWhiteSpace(x.CodeInternal)
                && (EF.Functions.Like(x.Name.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.Description.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.CodeInternal.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.Owner.FullName.ToLower(), $"%{searchString.ToLower()}%"));
            }
            else
            {
                Criteria = x => !string.IsNullOrWhiteSpace(x.CodeInternal);
            }
        }
    }
}