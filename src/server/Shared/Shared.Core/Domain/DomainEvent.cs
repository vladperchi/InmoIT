// --------------------------------------------------------------------------------------------------
// <copyright file="DomainEvent.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

namespace InmoIT.Shared.Core.Domain
{
    public abstract class DomainEvent : Event
    {
        protected DomainEvent(Guid addedId)
        {
            AddedId = addedId;
        }
    }
}