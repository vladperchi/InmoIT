// --------------------------------------------------------------------------------------------------
// <copyright file="CustomerFilterSpecification.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Shared.Core.Interfaces.Specifications;

using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Person.Core.Specifications
{
    public class CustomerFilterSpecification : Specification<Customer>
    {
        public CustomerFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = x => !string.IsNullOrWhiteSpace(x.Email)
                && (EF.Functions.Like(x.Name.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.SurName.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.PhoneNumber.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.Email.ToLower(), $"%{searchString.ToLower()}%"));
            }
            else
            {
                Criteria = x => !string.IsNullOrWhiteSpace(x.Email);
            }
        }
    }
}