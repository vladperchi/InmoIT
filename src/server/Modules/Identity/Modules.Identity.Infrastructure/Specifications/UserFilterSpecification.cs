// --------------------------------------------------------------------------------------------------
// <copyright file="UserFilterSpecification.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Shared.Core.Interfaces.Specifications;

using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Identity.Infrastructure.Specifications
{
    public class UserFilterSpecification : Specification<InmoUser>
    {
        public UserFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = x => EF.Functions.Like(x.FirstName.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.LastName.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.Email.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.PhoneNumber.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.UserName.ToLower(), $"%{searchString.ToLower()}%");
            }
            else
            {
                Criteria = _ => true;
            }
        }
    }
}