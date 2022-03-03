// --------------------------------------------------------------------------------------------------
// <copyright file="UpdateUserPictureRequest.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Shared.Dtos.Upload;

namespace InmoIT.Shared.Dtos.Identity.Users
{
    public class UpdateUserPictureRequest : FileUploadRequest
    {
        public bool DeleteCurrentImageUrl { get; set; } = false;
    }
}