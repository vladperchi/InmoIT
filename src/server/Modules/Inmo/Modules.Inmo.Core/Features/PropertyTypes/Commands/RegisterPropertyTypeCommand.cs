// --------------------------------------------------------------------------------------------------
// <copyright file="RegisterPropertyTypeCommand.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Upload;

using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.PropertyTypes.Commands
{
    public class RegisterPropertyTypeCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public FileUploadRequest FileUploadRequest { get; set; }
    }
}