// --------------------------------------------------------------------------------------------------
// <copyright file="InmoRoleClaim.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using InmoIT.Shared.Core.Contracts;
using InmoIT.Shared.Core.Domain;

using Microsoft.AspNetCore.Identity;

namespace InmoIT.Modules.Identity.Core.Entities
{
    public class InmoRoleClaim : IdentityRoleClaim<string>, IBaseEntity
    {
        public string Description { get; set; }

        public string Group { get; set; }

        public virtual InmoRole Role { get; set; }

        private List<Event> _domainEvents;

        public IReadOnlyCollection<Event> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(Event domainEvent)
        {
            _domainEvents ??= new List<Event>();
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(Event domainEvent)
        {
            _domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public InmoRoleClaim()
            : base()
        {
        }

        public InmoRoleClaim(string roleClaimDescription = null, string roleClaimGroup = null)
            : base()
        {
            Description = roleClaimDescription;
            Group = roleClaimGroup;
        }
    }
}