// --------------------------------------------------------------------------------------------------
// <copyright file="Message.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using InmoIT.Shared.Core.Utilities;

namespace InmoIT.Shared.Core.Domain
{
    public abstract class Message
    {
        public string MessageType { get; protected set; }

        public Guid AddedId { get; protected set; }

        protected Message()
        {
            MessageType = GetType().GetGenericTypeName();
        }
    }
}