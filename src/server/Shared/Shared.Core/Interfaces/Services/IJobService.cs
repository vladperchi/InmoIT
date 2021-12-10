// --------------------------------------------------------------------------------------------------
// <copyright file="IJobService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InmoIT.Shared.Core.Interfaces.Services
{
    public interface IJobService
    {
        string Enqueue(Expression<Func<Task>> methodCall);
    }
}