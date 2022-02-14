// --------------------------------------------------------------------------------------------------
// <copyright file="EntityReferenceService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Exceptions;
using InmoIT.Shared.Core.Integration.Application;
using InmoIT.Shared.Core.Interfaces.Contexts;
using Microsoft.Extensions.Localization;
using InmoIT.Shared.Core.Entities;

namespace InmoIT.Shared.Infrastructure.Services
{
    /// <inheritdoc cref = "IEntityReferenceService" />
    public class EntityReferenceService : IEntityReferenceService
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<EntityReferenceService> _localizer;

        public EntityReferenceService(
            IApplicationDbContext context,
            IStringLocalizer<EntityReferenceService> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<string> PropertyTrackAsync(string entityName)
        {
            if (string.IsNullOrWhiteSpace(entityName))
            {
                throw new EntityNameEmptyException(_localizer);
            }

            string referenceNumber;
            string monthYearString = DateTime.Now.ToString("MMyy");
            var record = _context.EntityReferences.FirstOrDefault(a => a.Entity == entityName && a.MonthYearString == monthYearString);
            if (record != null)
            {
                record.Increment();
                _context.EntityReferences.Update(record);
                referenceNumber = GenerateReferenceNumber(entityName, record.Count, monthYearString);
            }
            else
            {
                record = new EntityReference(entityName);
                _context.EntityReferences.Add(record);
                referenceNumber = GenerateReferenceNumber(entityName, record.Count, monthYearString);
            }

            await _context.SaveChangesAsync();
            return referenceNumber;
        }

        private string GenerateReferenceNumber(string entity, int count, string monthYearString)
        {
            return $"{entity[0]}-{monthYearString}-{count.ToString().PadLeft(5, '0')}";
        }
    }
}