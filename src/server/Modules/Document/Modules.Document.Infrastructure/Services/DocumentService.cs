// --------------------------------------------------------------------------------------------------
// <copyright file="DocumentService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InmoIT.Modules.Document.Core.Abstractions;
using InmoIT.Shared.Core.Integration.Document;
using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Document.Infrastructure.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentDbContext _context;

        public DocumentService(IDocumentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsDocumentTypeUsed(Guid documentTypeId)
        {
            return await _context.Documents.AnyAsync(x => x.DocumentTypeId == documentTypeId);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Documents.CountAsync();
        }
    }
}