// --------------------------------------------------------------------------------------------------
// <copyright file="DocumentType.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Document.Core.Entities
{
    public class DocumentType : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}