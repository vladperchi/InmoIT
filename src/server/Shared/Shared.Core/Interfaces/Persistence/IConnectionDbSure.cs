// --------------------------------------------------------------------------------------------------
// <copyright file="IConnectionDbSure.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;

namespace InmoIT.Shared.Core.Interfaces.Persistence
{
    public interface IConnectionDbSure
    {
        Task<string> MakeSure(string connectionString, string dataProvider = null);
    }
}