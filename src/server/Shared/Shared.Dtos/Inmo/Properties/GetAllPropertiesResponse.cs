// --------------------------------------------------------------------------------------------------
// <copyright file="GetAllPropertiesResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Inmo.Properties
{
    public record GetAllPropertiesResponse(Guid Id, string Name, string Address, string Description, decimal SquareMeter, int NumberRooms, int NumberBathrooms, decimal SalePrice, decimal RentPrice, decimal SaleTax, decimal IncomeTax, string CodeInternal, int Year, bool HasParking, bool IsActive, string OwnerName, Guid OwnerId, string TypeName, Guid PropertyTypeId)
    {
        public string PropertyImageCaption { get; set; }

        public string PropertyImageUrl { get; set; }

        public string PropertyImageCode { get; set; }
    }
}