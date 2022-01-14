// --------------------------------------------------------------------------------------------------
// <copyright file="RegisterOwnerCommand.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Upload;
using MediatR;

namespace InmoIT.Modules.Inmo.Core.Features.Owners.Commands
{
    public class RegisterOwnerCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; }

        public string SurName { get; set; }

        public string Address { get; set; }

        public string ImageUrl { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime Birthday { get; set; }

        public string Gender { get; set; }

        public string Group { get; set; }

        public bool IsActive { get; set; }

        public FileUploadRequest FileUploadRequest { get; set; }

        public string FileName => $"{Name}{PhoneNumber}";
    }
}