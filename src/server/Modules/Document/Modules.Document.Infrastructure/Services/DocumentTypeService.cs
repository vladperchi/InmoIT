// --------------------------------------------------------------------------------------------------
// <copyright file="DocumentTypeService.cs" company="InmoIT">
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
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly IDocumentDbContext _context;

        public DocumentTypeService(IDocumentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsDocumentTypeUsed(Guid id)
        {
            return await _context.DocumentTypes.AnyAsync(x => x.Id == id);
        }
    }
}