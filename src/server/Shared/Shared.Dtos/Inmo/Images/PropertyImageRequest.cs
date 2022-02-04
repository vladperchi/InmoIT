// --------------------------------------------------------------------------------------------------
// <copyright file="PropertyImageRequest.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Dtos.Inmo.Images
{
    public class PropertyImageRequest
    {
        public string FileName { get; set; }

        public string Caption { get; set; }

        public bool Enabled { get; set; }

        public string CodeImage { get; set; }

        public string ImageData { get; set; }
    }
}