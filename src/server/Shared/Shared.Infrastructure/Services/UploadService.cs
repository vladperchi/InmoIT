// --------------------------------------------------------------------------------------------------
// <copyright file="UploadService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Common.Enums;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Exceptions;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Dtos.Upload;
using InmoIT.Shared.Infrastructure.Extensions;
using InmoIT.Shared.Infrastructure.Helpers;
using Microsoft.Extensions.Localization;

namespace InmoIT.Shared.Infrastructure.Services
{
     public class UploadService : IUploadService
    {
        private readonly IStringLocalizer<UploadService> _localizer;

        public UploadService(IStringLocalizer<UploadService> localizer)
        {
            _localizer = localizer;
        }

        public Task<string> UploadAsync(FileUploadRequest request, FileType supportedFileType)
        {
            if (request.Data == null)
            {
                return Task.FromResult(string.Empty);
            }

            if (!supportedFileType.GetDescriptionList().Contains(request.Extension))
                throw new FileFormatInvalidException(_localizer);

            byte[] data = request.Data.ToByteArray();
            var memoryStream = new MemoryStream(data);
            if (memoryStream.Length > 0)
            {
                string folder = request.UploadStorageType.ToDescriptionString();
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    folder = folder.Replace(@"\", "/");
                }

                string folderName = Path.Combine("Files", folder);
                string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                bool exists = Directory.Exists(pathToSave);
                if (!exists)
                {
                    Directory.CreateDirectory(pathToSave);
                }

                string fileName = request.FileName.Trim('"');
                string fullPath = Path.Combine(pathToSave, fileName);
                string dbPath = Path.Combine(folderName, fileName);
                if (File.Exists(dbPath))
                {
                    dbPath = NextAvailableFilename(dbPath);
                    fullPath = NextAvailableFilename(fullPath);
                }

                using var fileStream = new FileStream(fullPath, FileMode.Create);
                memoryStream.CopyTo(fileStream);
                return Task.FromResult(dbPath);
            }
            else
            {
                return Task.FromResult(string.Empty);
            }
        }

        private static string NextAvailableFilename(string path)
        {
            if (!File.Exists(path))
            {
                return path;
            }

            if (Path.HasExtension(path))
            {
                return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path), StringComparison.Ordinal), PatternConstant.Number));
            }

            return GetNextFilename(path + PatternConstant.Number);
        }

        private static string GetNextFilename(string pattern)
        {
            string temp = string.Format(pattern, 1);

            if (!File.Exists(temp))
            {
                return temp;
            }

            int min = 1, max = 2;

            while (File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                int pivot = (max + min) / 2;
                if (File.Exists(string.Format(pattern, pivot)))
                {
                    min = pivot;
                }
                else
                {
                    max = pivot;
                }
            }

            return string.Format(pattern, max);
        }
    }
}