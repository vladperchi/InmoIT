// --------------------------------------------------------------------------------------------------
// <copyright file="GetAllPropertyImagesResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Inmo.Images
{
    public record GetAllPropertyImagesResponse(Guid Id, string ImageUrl, string Caption, bool Enabled, string CodeImage, Guid PropertyId);
}