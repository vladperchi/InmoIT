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
        [Description(@"Images\Flow\Owners")]
        Owner,

        [Description(@"Images\Flow\Properties")]
        Property,

        [Description(@"Images\Users")]
        User,

        [Description(@"Images\Documents")]
        Document
    }
}