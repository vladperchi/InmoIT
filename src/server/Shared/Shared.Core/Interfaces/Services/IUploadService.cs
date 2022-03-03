// --------------------------------------------------------------------------------------------------
// <copyright file="IUploadService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using InmoIT.Shared.Core.Common.Enums;
using InmoIT.Shared.Dtos.Upload;

namespace InmoIT.Shared.Core.Interfaces.Services
{
    public interface IUploadService
    {
        Task<string> UploadAsync(FileUploadRequest request, FileType supportedFileType);

        Task<bool> RemoveFileImage(UploadStorageType pathFolder, string currentImageUrl);
    }
}