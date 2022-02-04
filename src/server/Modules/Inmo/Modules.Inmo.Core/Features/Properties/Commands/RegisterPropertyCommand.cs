// --------------------------------------------------------------------------------------------------
// <copyright file="RegisterPropertyCommand.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Wrapper;
using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.Properties.Commands
{
    public class RegisterPropertyCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public int SquareMeter { get; set; }

        public int NumberRooms { get; set; }

        public int NumberBathrooms { get; set; }

        public decimal SalePrice { get; set; }

        public decimal RentPrice { get; set; }

        public decimal SaleTax { get; set; }

        public decimal IncomeTax { get; set; }

        public string CodeInternal { get; set; }

        public int Year { get; set; }

        public bool HasParking { get; set; }

        public bool IsActive { get; set; }

        public Guid OwnerId { get; set; }

        public Guid PropertyTypeId { get; set; }
    }
}