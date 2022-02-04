// --------------------------------------------------------------------------------------------------
// <copyright file="OwnerFilterSpecification.cs" company="InmoIT">
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
    public class OwnerFilterSpecification : Specification<Owner>
    {
        public OwnerFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = x => !string.IsNullOrWhiteSpace(x.Email) && x.IsActive
                && (EF.Functions.Like(x.Name.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.SurName.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.PhoneNumber.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.Email.ToLower(), $"%{searchString.ToLower()}%"));
            }
            else
            {
                Criteria = x => !string.IsNullOrWhiteSpace(x.Email) && x.IsActive;
            }
        }
    }
}