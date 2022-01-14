// --------------------------------------------------------------------------------------------------
// <copyright file="ClearCartCommand.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Wrapper;
using MediatR;

namespace InmoIT.Modules.Person.Core.Features.Carts.Commands
{
    public class ClearCartCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; }

        public ClearCartCommand(Guid cartId)
        {
            Id = cartId;
        }
    }
}