// --------------------------------------------------------------------------------------------------
// <copyright file="RemovePropertyImageCommand.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Wrapper;
using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.Images.Commands
{
    public class RemovePropertyImageCommand : IRequest<Result>
    {
        public Guid Id { get; set; }

        public RemovePropertyImageCommand(Guid propertyId)
        {
            Id = propertyId;
        }
    }
}