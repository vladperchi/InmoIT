// --------------------------------------------------------------------------------------------------
// <copyright file="FileUploadRequest.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Dtos.Upload
{
    public class FileUploadRequest
    {
        public string FileName { get; set; }

        public string Extension { get; set; }

        public UploadStorageType UploadStorageType { get; set; }

        public string Data { get; set; }
    }
}