﻿// --------------------------------------------------------------------------------------------------
// <copyright file="AddImageCommand.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Upload;
using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.Images.Commands
{
    public class AddImageCommand : IRequest<Result<Guid>>
    {
        public string ImageUrl { get; set; }

        public string Caption { get; set; }

        public bool Enabled { get; set; }

        public string CodeImage { get; set; }

        public Guid PropertyId { get; set; }

        public List<string> ImageData { get; set; }

        public FileUploadRequest FileUploadRequest { get; set; }
    }
}