// --------------------------------------------------------------------------------------------------
// <copyright file="UpdateImageCommand.cs" company="InmoIT">
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
    public class UpdateImageCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }

        public string FileName { get; set; }

        public string Caption { get; set; }

        public bool Enabled { get; set; }

        public string CodeImage { get; set; }

        public string ImageUrl { get; set; }

        public bool DeleteCurrentImageUrl { get; set; } = false;

        public string Data { get; set; }
    }
}