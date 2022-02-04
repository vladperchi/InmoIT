// --------------------------------------------------------------------------------------------------
// <copyright file="TransactionType.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace InmoIT.Shared.Core.Common.Enums
{
    public enum TransactionType
    {
        [Description("Rent")]
        Purchase,

        [Description("Sale")]
        Sale
    }
}