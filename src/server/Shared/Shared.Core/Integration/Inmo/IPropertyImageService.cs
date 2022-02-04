// --------------------------------------------------------------------------------------------------
// <copyright file="IPropertyImageService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.Images;

namespace InmoIT.Shared.Core.Integration.Inmo
{
    public interface IPropertyImageService
    {
        Task<Result<GetPropertyImageByPropertyIdResponse>> GetDetailsPropertyImageAsync(Guid propertyId);

        Task<string> GenerateFileName(int length);

        Task<int> GetCountAsync();
    }
}