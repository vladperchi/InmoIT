// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyImage.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Inmo.Core.Entities
{
    public class PropertyImage : BaseEntity
    {
        public string ImageUrl { get; set; }

        public string Caption { get; set; }

        public bool Enabled { get; set; }

        public string CodeImage { get; set; }

        public Guid PropertyId { get; set; }

        public virtual Property Property { get; set; }

        public PropertyImage ClearPathImageUrl()
        {
            ImageUrl = string.Empty;
            return this;
        }
    }
}