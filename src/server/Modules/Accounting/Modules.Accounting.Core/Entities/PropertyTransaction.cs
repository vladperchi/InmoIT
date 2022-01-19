// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyTransaction.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Common.Enums;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Accounting.Core.Entities
{
    public class PropertyTransaction : BaseEntity
    {
        public PropertyTransaction(Guid propertyId, TransactionType type, string referenceNumber)
        {
            PropertyId = propertyId;
            Type = type;
            ReferenceNumber = referenceNumber;
            Timestamp = DateTime.Now;
        }

        public Guid PropertyId { get; private set; }

        public TransactionType Type { get; private set; }

        public string ReferenceNumber { get; private set; }

        public DateTime Timestamp { get; private set; }
    }
}