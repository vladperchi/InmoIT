// --------------------------------------------------------------------------------------------------
// <copyright file="CachingBehavior.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Exceptions;
using InmoIT.Shared.Core.Interfaces.Serialization.Serializer;
using InmoIT.Shared.Core.Queries;
using InmoIT.Shared.Core.Settings;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InmoIT.Shared.Core.Behaviors
{
    public class CachingBehavior
    {
    }

    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICacheable
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger _logger;
        private readonly IStringLocalizer<CachingBehavior> _localizer;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly CacheSettings _settings;

        public CachingBehavior(IDistributedCache cache, ILogger<TResponse> logger, IOptions<CacheSettings> settings, IStringLocalizer<CachingBehavior> localizer, IJsonSerializer jsonSerializer)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _localizer = localizer;
            _jsonSerializer = jsonSerializer;
            _settings = settings.Value;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response;
            if (request.SkipCache)
            {
                _logger.LogInformation(string.Format(_localizer["Skip Cache for -> '{0}'."], request.CacheKey));
                return await next();
            }

            async Task<TResponse> GetResponseAndAddToCache()
            {
                response = await next();
                var expiration = request.Expiration ?? TimeSpan.FromHours(_settings.Expiration);
                if (expiration <= TimeSpan.Zero)
                {
                    throw new CustomException(_localizer["Cache Expiration must be greater than 0."], statusCode: HttpStatusCode.BadRequest);
                }

                var options = new DistributedCacheEntryOptions { SlidingExpiration = expiration };
                byte[] serializedData = Encoding.Default.GetBytes(_jsonSerializer.Serialize(response));
                await _cache.SetAsync(request.CacheKey, serializedData, options, cancellationToken);
                return response;
            }

            byte[] cachedResponse = !string.IsNullOrWhiteSpace(request.CacheKey) ? await _cache.GetAsync(request.CacheKey, cancellationToken) : null;
            if (cachedResponse != null)
            {
                response = _jsonSerializer.Deserialize<TResponse>(Encoding.Default.GetString(cachedResponse));
                _logger.LogInformation(string.Format(_localizer["Fetched from Cache -> '{0}'."], request.CacheKey));
            }
            else
            {
                response = await GetResponseAndAddToCache();
                _logger.LogInformation(string.Format(_localizer["Added to Cache -> '{0}'."], request.CacheKey));
            }

            return response;
        }
    }
}