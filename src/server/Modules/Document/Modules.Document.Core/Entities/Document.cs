// --------------------------------------------------------------------------------------------------
// <copyright file="Document.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Document.Core.Entities
{
    public class Document : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPublic { get; set; } = true;

        public string FileUrl { get; set; }

        public Guid DocumentTypeId { get; set; }

        public virtual DocumentType DocumentType { get; set; }

        public Guid PropertyId { get; set; }
    }
}