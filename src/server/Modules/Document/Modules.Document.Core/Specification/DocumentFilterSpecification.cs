// --------------------------------------------------------------------------------------------------
// <copyright file="DocumentFilterSpecification.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Core.Interfaces.Specifications;
using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Document.Core.Specification
{
    public class DocumentFilterSpecification : Specification<Entities.Document>
    {
        public DocumentFilterSpecification(string searchString, string userId)
        {
            Includes.Add(x => x.DocumentType);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = x => x.IsPublic
                && (EF.Functions.Like(x.Name.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.Description.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.DocumentType.Name.ToLower(), $"%{searchString.ToLower()}%"));
            }
            else
            {
                Criteria = _ => true;
            }
        }
    }
}