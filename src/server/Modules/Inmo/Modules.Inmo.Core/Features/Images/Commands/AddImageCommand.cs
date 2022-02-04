// --------------------------------------------------------------------------------------------------
// <copyright file="AddImageCommand.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Inmo.Images;
using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.Images.Commands
{
    public class AddImageCommand : IRequest<Result<List<Guid>>>
    {
        public Guid PropertyId { get; set; }

        public List<PropertyImageRequest> PropertyImageList { get; set; }
    }
}