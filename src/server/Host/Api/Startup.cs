// --------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Accounting.Api.Extensions;
using InmoIT.Modules.Identity.Api.Extensions;
using InmoIT.Modules.Inmo.Api.Extensions;
using InmoIT.Modules.Person.Api.Extensions;
using InmoIT.Shared.Core.Extensions;
using InmoIT.Shared.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InmoIT.Api
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDistributedMemoryCache()
                .AddSerialization(_config)
                .AddSharedInfrastructure(_config)
                .AddSharedApplication(_config)
                .AddIdentityModule(_config)
                .AddInmoModule(_config)
                .AddCustomerModule(_config)
                .AddAccountingModule(_config);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSharedInfrastructure(_config);
        }
    }
}