// --------------------------------------------------------------------------------------------------
// <copyright file="GetAllDocumentTypesResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Document.DocumentTypes
{
    public record GetAllDocumentTypesResponse(Guid Id, string Name, string Description);
}