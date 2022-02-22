// --------------------------------------------------------------------------------------------------
// <copyright file="ExceptionHandlerMiddleware.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Exceptions;
using InmoIT.Shared.Core.Interfaces.Serialization.Serializer;
using InmoIT.Shared.Core.Interfaces.Services.Identity;
using InmoIT.Shared.Core.Serialization;
using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Core.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Serilog.Context;

namespace InmoIT.Shared.Infrastructure.Middlewares
{
    /// <inheritdoc cref = "IMiddleware" />
    internal class ExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly IStringLocalizer<ExceptionHandlerMiddleware> _localizer;
        private readonly SerializationSettings _serializationSettings;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ICurrentUser _currentUser;

        public ExceptionHandlerMiddleware(
            ILogger<ExceptionHandlerMiddleware> logger,
            IStringLocalizer<ExceptionHandlerMiddleware> localizer,
            IOptions<SerializationSettings> serializationSettings,
            IJsonSerializer jsonSerializer,
            ICurrentUser currentUser)
        {
            _logger = logger;
            _localizer = localizer;
            _serializationSettings = serializationSettings.Value;
            _jsonSerializer = jsonSerializer;
            _currentUser = currentUser;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate request)
        {
            try
            {
                await request(context);
            }
            catch (Exception exception)
            {
                context.Request.EnableBuffering();
                Stream body = context.Request.Body;
                byte[] buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
                await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                string requestBody = Encoding.UTF8.GetString(buffer);
                body.Seek(0, SeekOrigin.Begin);
                context.Request.Body = body;

                if (!string.IsNullOrEmpty(requestBody) && context.Request.Path.ToString() != "/api/tokens/")
                {
                    requestBody = " Body: " + requestBody + Environment.NewLine;
                }
                else
                {
                    requestBody = string.Empty;
                }

                string email = _currentUser.GetUserEmail() is string userEmail ? userEmail : "Anonymous";
                LogContext.PushProperty("UserEmail", email);
                var userId = _currentUser.GetUserId();
                if (userId != Guid.Empty)
                {
                    LogContext.PushProperty("UserId", userId);
                }

                string errorId = Guid.NewGuid().ToString();
                LogContext.PushProperty("ErrorId", errorId);
                LogContext.PushProperty("StackTrace", exception.StackTrace);
                var errorResult = new ErrorResult<string>
                {
                    Source = exception.TargetSite?.DeclaringType?.FullName,
                    Exception = exception.Message.Trim(),
                    ErrorId = errorId,
                    RemoteIP = $"{context.Connection.RemoteIpAddress}",
                    Schema = context.Request.Scheme ?? string.Empty,
                    Host = $"{context.Request.Host}",
                    Method = context.Request.Method ?? string.Empty,
                    Path = $"{context.Request.Path}",
                    QueryString = $"{context.Request.QueryString}::{requestBody}",
                    StatusCode = context.Response.StatusCode,
                    SupportMessage = string.Format(_localizer["Please provide the ErrorId: {0} to the support team for further analysis"], errorId),
                };
                errorResult.Messages!.Add(exception.Message);
                var response = context.Response;
                response.ContentType = "application/json";
                if (exception is not CustomException && exception.InnerException != null)
                {
                    while (exception.InnerException != null)
                    {
                        exception = exception.InnerException;
                    }
                }

                switch (exception)
                {
                    case CustomException ex:
                        response.StatusCode = errorResult.StatusCode = (int)ex.StatusCode;
                        errorResult.Messages = ex.ErrorMessages;
                        break;

                    case KeyNotFoundException:
                        response.StatusCode = errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    default:
                        response.StatusCode = errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                string result = string.Empty;
                if (_serializationSettings.UseNewtonsoftJson)
                {
                    result = _jsonSerializer.Serialize(errorResult, new JsonSerializerSettingsOptions
                    {
                        JsonSerializerSettings = { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                    });
                }
                else if (_serializationSettings.UseSystemTextJson)
                {
                    result = _jsonSerializer.Serialize(errorResult, new JsonSerializerSettingsOptions
                    {
                        JsonSerializerOptions = { DictionaryKeyPolicy = JsonNamingPolicy.CamelCase, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                    });
                }

                _logger.LogError($"Request failed::{errorResult}");
                await response.WriteAsync(result);
            }
        }
    }
}