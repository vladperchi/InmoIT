﻿// --------------------------------------------------------------------------------------------------
// <copyright file="IDocumentTypeService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Document.DocumentTypes;

namespace InmoIT.Shared.Core.Integration.Document
{
    public interface IDocumentTypeService
    {
        Task<Result<GetDocumentTypeByIdResponse>> GetDetailsAsync(Guid documentTypeId);

        Task<bool> IsDocumentTypeUsed(Guid documentTypeId);
    }
}