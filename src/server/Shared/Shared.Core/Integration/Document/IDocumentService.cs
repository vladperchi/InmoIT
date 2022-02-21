// --------------------------------------------------------------------------------------------------
// <copyright file="IDocumentService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using System;

namespace InmoIT.Shared.Core.Integration.Document
{
    public interface IDocumentService
    {
        Task<bool> IsDocumentTypeUsed(Guid documentTypeId);

        Task<int> GetCountAsync();
    }
}