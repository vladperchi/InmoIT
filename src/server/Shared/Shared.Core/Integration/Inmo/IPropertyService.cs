﻿// --------------------------------------------------------------------------------------------------
// <copyright file="IPropertyService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.Properties;

namespace InmoIT.Shared.Core.Integration.Inmo
{
    public interface IPropertyService
    {
        Task<Result<GetPropertyByIdResponse>> GetDetailsPropertyAsync(Guid propertyId);

        Task<Result<Guid>> ChangeStatusPropertyAsync(Guid propertyId, bool status);

        Task<Result<Guid>> RemovePropertyAsync(Guid propertyId);

        Task<bool> IsPropertyTypeUsed(Guid propertyTypeId);

        Task<int> GetCountAsync();
    }
}