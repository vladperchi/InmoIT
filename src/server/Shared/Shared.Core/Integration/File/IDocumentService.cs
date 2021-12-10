﻿// --------------------------------------------------------------------------------------------------
// <copyright file="IDocumentService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.File.Documents;

namespace InmoIT.Shared.Core.Integration.File
{
    public interface IDocumentService
    {
        Task<Result<GetDocumentByIdResponse>> GetDetailsAsync(Guid documentId);
    }
}