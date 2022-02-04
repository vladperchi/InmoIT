// --------------------------------------------------------------------------------------------------
// <copyright file="GetPropertyByIdResponse.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Dtos.Inmo.Properties
{
    public record GetPropertyByIdResponse(Guid Id, string Name, string Address, string Description, int SquareMeter, int NumberRooms, int NumberBathrooms, decimal SalePrice, decimal RentPrice, decimal SaleTax, decimal IncomeTax, string CodeInternal, int Year, bool HasParking, bool IsActive, string OwnerName, Guid OwnerId, string TypeName, Guid PropertyType)
    {
        public string PropertyImageCaption { get; set; }

        public string PropertyImageUrl { get; set; }

        public string PropertyImageCode { get; set; }

        public bool PropertyImageEnabled { get; set; }
    }
}