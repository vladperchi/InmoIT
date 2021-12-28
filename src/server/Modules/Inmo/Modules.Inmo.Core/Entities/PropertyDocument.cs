// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyDocument.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Inmo.Core.Entities
{
    public class PropertyDocument : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string FileUrl { get; set; }

        public bool IsPublic { get; set; } = true;

        public Guid DocumentTypeId { get; set; }

        public virtual DocumentType DocumentType { get; set; }

        public Guid PropertyId { get; set; }

        public virtual Property Property { get; set; }
    }
}