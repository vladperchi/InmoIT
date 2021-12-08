// --------------------------------------------------------------------------------------------------
// <copyright file="IEntity.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Core.Contracts
{
    public interface IEntity<TEntityId> : IEntity
    {
        public TEntityId Id { get; set; }
    }

    public interface IEntity
    {
    }
}