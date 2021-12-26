// --------------------------------------------------------------------------------------------------
// <copyright file="PaginatedCustomerFilterValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Person.Core.Entities;
using InmoIT.Shared.Core.Features.Queries.Validators;
using InmoIT.Shared.Dtos.Customers;
using Microsoft.Extensions.Localization;

namespace InmoIT.Modules.Person.Core.Features.Customers.Queries.Validators
{
    public class PaginatedCustomerFilterValidator : PaginatedFilterValidator<Guid, Customer, PaginatedCustomerFilter>
    {
        public PaginatedCustomerFilterValidator(IStringLocalizer<PaginatedCustomerFilterValidator> localizer)
            : base(localizer)
        {
        }
    }
}