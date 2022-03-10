// --------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Threading;
using InmoIT.Shared.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace InmoIT.Api
{
  public class Program
  {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            try
            {
                Log.Information("Server Starting Up...");
                Log.Information("Welcome {Name} from thread {ThreadId}", Environment.UserName, Thread.CurrentThread.ManagedThreadId);
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Something went wrong. Server failed...");
            }
            finally
            {
                Log.Information("Server Shutting down...");
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            }).ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Debug);

                // logging.AddDebug();
            });
    }
}