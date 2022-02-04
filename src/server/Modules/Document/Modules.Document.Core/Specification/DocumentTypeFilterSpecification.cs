// --------------------------------------------------------------------------------------------------
// <copyright file="DocumentTypeFilterSpecification.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Document.Core.Entities;
using InmoIT.Shared.Core.Interfaces.Specifications;
using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Document.Core.Specification
{
    public class DocumentTypeFilterSpecification : Specification<DocumentType>
    {
        public DocumentTypeFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = x => x.IsActive
                && (EF.Functions.Like(x.Name.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.Description.ToLower(), $"%{searchString.ToLower()}%"));
            }
            else
            {
                Criteria = _ => true;
            }
        }
    }
}