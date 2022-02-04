// --------------------------------------------------------------------------------------------------
// <copyright file="IPropertyTypeService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.PropertyTypes;

namespace InmoIT.Shared.Core.Integration.Inmo
{
    public interface IPropertyTypeService
    {
        Task<Result<GetPropertyTypeByIdResponse>> GetDetailsPropertyTypeAsync(Guid propertyTypeId);

        Task<Result<Guid>> RemovePropertyTypeAsync(Guid propertyTypeId);

        Task<string> GenerateFileName(int length);

        Task<int> GetCountAsync();
    }
}