// --------------------------------------------------------------------------------------------------
// <copyright file="GetCustomerImageQuery.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Wrapper;
using MediatR;

namespace InmoIT.Modules.Person.Core.Features.Customers.Queries
{
    public class GetCustomerImageQuery : IRequest<Result<string>>
    {
        public Guid Id { get; }

        public GetCustomerImageQuery(Guid customerId)
        {
            Id = customerId;
        }
    }
}