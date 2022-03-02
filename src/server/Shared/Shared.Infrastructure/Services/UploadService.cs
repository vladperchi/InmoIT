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
using System.Text.RegularExpressions;
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
    /// <inheritdoc cref = "IUploadService" />
    public class UploadService : IUploadService
    {
        private readonly IStringLocalizer<UploadService> _localizer;

        public UploadService(IStringLocalizer<UploadService> localizer)
        {
            _localizer = localizer;
        }

        public async Task<string> UploadAsync(FileUploadRequest request, FileType supportedFileType)
        {
            if (request is null || request.Data is null)
            {
                return string.Empty;
            }

            if (request.Extension is null || !supportedFileType.GetDescriptionList().Contains(request.Extension.ToLower()))
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
                fileName = RemoveSpecialCharacters(fileName);
                string fullPath = Path.Combine(pathToSave, fileName);
                string dbPath = Path.Combine(folderName, fileName);
                if (File.Exists(dbPath))
                {
                    dbPath = NextAvailableFilename(dbPath);
                    fullPath = NextAvailableFilename(fullPath);
                }

                using var fileStream = new FileStream(fullPath, FileMode.Create);
                await memoryStream.CopyToAsync(fileStream);
                return dbPath.Replace("\\", "/");
            }
            else
            {
                return string.Empty;
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

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled);
        }

        public Task<bool> RemoveFileImage(UploadStorageType pathFolder, string currentImageUrl)
        {
            bool removedFile = false;
            string folder = pathFolder.ToDescriptionString();
            string folderName = Path.Combine("Files", folder);
            string pathToDelete = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            bool exists = Directory.Exists(pathToDelete);
            if (exists && File.Exists(currentImageUrl))
            {
                File.Delete(currentImageUrl);
                removedFile = true;
            }

            return Task.FromResult(removedFile);
        }
    }
}