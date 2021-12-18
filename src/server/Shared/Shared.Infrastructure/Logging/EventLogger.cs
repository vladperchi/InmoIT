// --------------------------------------------------------------------------------------------------
// <copyright file="EventLogger.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using InmoIT.Shared.Core.Domain;
using InmoIT.Shared.Core.Entities;
using InmoIT.Shared.Core.Interfaces.Contexts;
using InmoIT.Shared.Core.Interfaces.Services.Identity;
using InmoIT.Shared.Core.Interfaces.Serialization.Serializer;

namespace InmoIT.Shared.Infrastructure.Logging
{
    internal class EventLogger : IEventLogger
    {
        private readonly ICurrentUser _user;
        private readonly IApplicationDbContext _context;
        private readonly IJsonSerializer _jsonSerializer;

        public EventLogger(
            ICurrentUser user,
            IApplicationDbContext context,
            IJsonSerializer jsonSerializer)
        {
            _user = user;
            _context = context;
            _jsonSerializer = jsonSerializer;
        }

        public async Task SaveAsync<T>(T @event, (string oldValues, string newValues) changes)
            where T : Event
        {
            if (@event is EventLog eventLog)
            {
                await _context.EventLogs.AddAsync(eventLog);
                await _context.SaveChangesAsync();
            }
            else
            {
                string serializedData = _jsonSerializer.Serialize(@event, @event.GetType());

                string userEmail = _user.GetUserEmail();
                if (string.IsNullOrWhiteSpace(userEmail))
                {
                    userEmail = "Anonymous";
                }

                var userId = _user.GetUserId();
                var thisEventLog = new EventLog(
                    @event,
                    serializedData,
                    changes,
                    userEmail,
                    userId);
                await _context.EventLogs.AddAsync(thisEventLog);
                await _context.SaveChangesAsync();
            }
        }
    }
}