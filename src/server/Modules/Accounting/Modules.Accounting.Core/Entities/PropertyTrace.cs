// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyTrace.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations.Schema;
using InmoIT.Shared.Core.Common.Enums;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Accounting.Core.Entities
{
    public class PropertyTrace : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }

        public decimal Tax { get; set; }

        public TransactionType TransactionType { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public Guid PropertyId { get; set; }

        [NotMapped]
        public decimal Tolal => Value + Tax;
    }
}