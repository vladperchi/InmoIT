// --------------------------------------------------------------------------------------------------
// <copyright file="UploadStorageType.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace InmoIT.Shared.Dtos.Upload
{
    public enum UploadStorageType
    {
        [Description(@"Images\User\Staff")]
        Staff,

        [Description(@"Images\Inmo\Owners")]
        Owner,

        [Description(@"Images\Inmo\Properties")]
        Property,

        [Description(@"Images\Inmo\PropertyTypes")]
        PropertyType,

        [Description(@"Images\Person\Customers")]
        Customer,

        [Description(@"Documents\Contract\Sale")]
        Sale,

        [Description(@"Documents\Contract\Rent")]
        Rent
    }
}