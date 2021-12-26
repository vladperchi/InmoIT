// --------------------------------------------------------------------------------------------------
// <copyright file="ISpecification.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using InmoIT.Shared.Core.Contracts;

namespace InmoIT.Shared.Core.Interfaces.Specifications
{
    public interface ISpecification<T>
        where T : class, IEntity
    {
        Expression<Func<T, bool>> Criteria { get; }

        List<Expression<Func<T, object>>> Includes { get; }

        List<string> IncludeStrings { get; }
    }
}