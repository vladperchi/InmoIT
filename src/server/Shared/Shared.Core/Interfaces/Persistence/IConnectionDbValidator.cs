// --------------------------------------------------------------------------------------------------
// <copyright file="IConnectionDbValidator.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;

namespace InmoIT.Shared.Core.Interfaces.Persistence
{
    public interface IConnectionDbValidator
    {
        Task<bool> TryValidate(string connectionString = null, string dataProvider = null);
    }
}