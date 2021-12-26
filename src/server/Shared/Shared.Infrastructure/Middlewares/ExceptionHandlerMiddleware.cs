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
using InmoIT.Shared.Core.Serialization;
using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Core.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json.Serialization;

namespace InmoIT.Shared.Infrastructure.Middlewares
{
    internal class ExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly SerializationSettings _serializationSettings;
        private readonly IJsonSerializer _jsonSerializer;

        public ExceptionHandlerMiddleware(
            ILogger<ExceptionHandlerMiddleware> logger,
            IOptions<SerializationSettings> serializationSettings,
            IJsonSerializer jsonSerializer)
        {
            _logger = logger;
            _serializationSettings = serializationSettings.Value;
            _jsonSerializer = jsonSerializer;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate request)
        {
            try
            {
                await request(context);
            }
            catch (Exception exception)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                if (exception is not CustomException && exception.InnerException != null)
                {
                    while (exception.InnerException != null)
                    {
                        exception = exception.InnerException;
                    }
                }

                context.Request.EnableBuffering();
                Stream body = context.Request.Body;
                byte[] buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
                await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                string requestBody = Encoding.UTF8.GetString(buffer);
                body.Seek(0, SeekOrigin.Begin);
                context.Request.Body = body;

                if (requestBody != string.Empty)
                {
                    if (context.Request.Path.ToString() != "/api/tokens/")
                    {
                        requestBody = " Body: " + requestBody + Environment.NewLine;
                    }
                    else if (context.Request.Path.ToString() != "/api/tokens/")
                    {
                        requestBody = string.Empty;
                    }
                }

                _logger.LogError(
                $"Exception: {exception.Message}{Environment.NewLine}" +
                    $"  RemoteIP: {context.Connection.RemoteIpAddress}{Environment.NewLine}" +
                    $"  Schema: {context.Request.Scheme}{Environment.NewLine}" +
                    $"  Host: {context.Request.Host}{Environment.NewLine}" +
                    $"  Method: {context.Request.Method}{Environment.NewLine}" +
                    $"  Path: {context.Request.Path}{Environment.NewLine}" +
                    $"  Query String: {context.Request.QueryString}{Environment.NewLine}" + requestBody +
                    $"  Response Status Code: {context.Response?.StatusCode}{Environment.NewLine}");

                var responseModel = await ErrorResult<string>.ReturnErrorAsync(exception.Message);
                responseModel.Source = exception.Source;
                responseModel.Exception = exception.Message;

                switch (exception)
                {
                    case CustomException ex:
                        response.StatusCode = responseModel.ErrorCode = (int)ex.StatusCode;
                        responseModel.Messages = ex.ErrorMessages;
                        break;

                    case KeyNotFoundException:
                        response.StatusCode = responseModel.ErrorCode = (int)HttpStatusCode.NotFound;
                        break;

                    default:
                        response.StatusCode = responseModel.ErrorCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                string result = string.Empty;
                if (_serializationSettings.UseNewtonsoftJson)
                {
                    result = _jsonSerializer.Serialize(responseModel, new JsonSerializerSettingsOptions
                    {
                        JsonSerializerSettings = { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                    });
                }
                else if (_serializationSettings.UseSystemTextJson)
                {
                    result = _jsonSerializer.Serialize(responseModel, new JsonSerializerSettingsOptions
                    {
                        JsonSerializerOptions = { DictionaryKeyPolicy = JsonNamingPolicy.CamelCase, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                    });
                }

                await response.WriteAsync(result);
            }
        }
    }
}