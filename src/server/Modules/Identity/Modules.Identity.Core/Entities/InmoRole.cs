// --------------------------------------------------------------------------------------------------
// <copyright file="InmoRole.cs" company="InmoIT">
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
    public class InmoRole : IdentityRole, IEntity<string>, IBaseEntity
    {
        public InmoRole()
            : base()
        {
            RoleClaims = new HashSet<InmoRoleClaim>();
        }

        public InmoRole(string roleName, string roleDescription = null)
            : base(roleName)
        {
            RoleClaims = new HashSet<InmoRoleClaim>();
            Description = roleDescription;
        }

        public string Description { get; set; }

        public virtual ICollection<InmoRoleClaim> RoleClaims { get; set; }

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
    }
}