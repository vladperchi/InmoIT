// --------------------------------------------------------------------------------------------------
// <copyright file="RemovePropertyCommand.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Wrapper;
using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.Properties.Commands
{
    public class RemovePropertyCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }

        public RemovePropertyCommand(Guid propertyId)
        {
            Id = propertyId;
        }
    }
}