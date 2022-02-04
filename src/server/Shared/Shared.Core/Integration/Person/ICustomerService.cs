// --------------------------------------------------------------------------------------------------
// <copyright file="ICustomerService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Person.Customers;

namespace InmoIT.Shared.Core.Integration.Person
{
    public interface ICustomerService
    {
        Task<Result<GetCustomerByIdResponse>> GetDetailsCustomerAsync(Guid customerId);

        Task<Result<Guid>> RemoveCustomerAsync(Guid customerId);

        Task<string> GenerateFileName(int length);

        Task<int> GetCountAsync();
    }
}