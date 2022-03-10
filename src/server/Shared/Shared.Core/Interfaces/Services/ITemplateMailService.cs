// --------------------------------------------------------------------------------------------------
// <copyright file="ITemplateMailService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.Threading.Tasks;

using InmoIT.Shared.Core.Wrapper;

namespace InmoIT.Shared.Core.Interfaces.Services
{
    public interface ITemplateMailService
    {
        Task<string> GenerateEmailTemplate<T>(string name, T emailModel);
    }
}