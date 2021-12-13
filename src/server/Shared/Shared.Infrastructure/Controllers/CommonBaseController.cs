// --------------------------------------------------------------------------------------------------
// <copyright file="CommonBaseController.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace InmoIT.Shared.Infrastructure.Controllers
{
    [ApiController]
    [Route(BasePath + "/[controller]")]
    public abstract class CommonBaseController : ControllerBase
    {
        protected internal const string BasePath = "api/v{version:apiVersion}";

        private IMediator _mediatorInstance;

        protected IMediator Mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();

        private IMapper _mapperInstance;

        protected IMapper Mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();
    }
}