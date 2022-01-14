// --------------------------------------------------------------------------------------------------
// <copyright file="UpdatePropertyCommand.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Upload;
using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.Properties.Commands
{
    public class UpdatePropertyCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal Tax { get; set; }

        public string CodeInternal { get; set; }

        public int Year { get; set; }

        public bool IsActive { get; set; } = true;

        public Guid OwnerId { get; set; }

        public FileUploadRequest FileUploadRequest { get; set; }

        public string FileName => $"{CodeInternal}{Year}";
    }
}