// --------------------------------------------------------------------------------------------------
// <copyright file="IEntityReferenceService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;

namespace InmoIT.Shared.Core.Integration.Application
{
    public interface IEntityReferenceService
    {
        public Task<string> TrackAsync(string entityName);
    }
}