// --------------------------------------------------------------------------------------------------
// <copyright file="FileType.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace InmoIT.Shared.Core.Common.Enums
{
    public enum FileType
    {
        [Description(".jpg|.png|.jpeg")]
        Image,

        [Description(".doc|.docx|.pdf")]
        Document
    }
}