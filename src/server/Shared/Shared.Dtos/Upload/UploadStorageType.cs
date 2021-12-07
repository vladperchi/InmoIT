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
        [Description(@"Images\Properties")]
        Property,

        [Description(@"Images\Profiles")]
        Profile,

        [Description(@"Documents")]
        Document
    }
}