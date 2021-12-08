// --------------------------------------------------------------------------------------------------
// <copyright file="Event.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace InmoIT.Shared.Core.Domain
{
    public abstract class Event : Message, INotification
    {
        [NotMapped]
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public IEnumerable<Type> RelatedEntities { get; protected set; }

        public DateTime Timestamp { get; private set; }

        public string Description { get; set; }

        protected Event(string description = null)
        {
            Timestamp = DateTime.Now;
            if (string.IsNullOrWhiteSpace(description))
            {
                Description = description;
            }
        }
    }
}