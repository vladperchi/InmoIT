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

        public decimal Price { get; set; }

        public decimal Tax { get; set; }

        public string CodeInternal { get; set; }

        public int Year { get; set; }

        public Guid OwnerId { get; set; }

        public virtual Owner Owner { get; set; }
    }
}