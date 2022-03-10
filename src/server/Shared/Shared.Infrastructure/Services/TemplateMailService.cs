// --------------------------------------------------------------------------------------------------
// <copyright file="TemplateMailService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.IO;
using System.Text;
using System;
using System.Threading.Tasks;
using InmoIT.Shared.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using RazorEngineCore;
using Microsoft.Extensions.Localization;

namespace InmoIT.Shared.Infrastructure.Services
{
    public class TemplateMailService : ITemplateMailService
    {
        private readonly IStringLocalizer<TemplateMailService> _localizer;
        private readonly ILogger<TemplateMailService> _logger;

        public TemplateMailService(
            IStringLocalizer<TemplateMailService> localizer,
            ILogger<TemplateMailService> logger)
        {
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<string> GenerateEmailTemplate<T>(string name, T emailModel)
        {
            string result = string.Empty;
            try
            {
                string template = GetTemplate(name);
                IRazorEngine razorEngine = new RazorEngine();
                IRazorEngineCompiledTemplate changedTemplate = razorEngine.Compile(template);
                result = await changedTemplate.RunAsync(emailModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(_localizer["An error occurred while generated email template {0}. Error message: {1}"], name, ex.Message));
            }

            return result;
        }

        private string GetTemplate(string name)
        {
            string folder = Path.Combine(Directory.GetCurrentDirectory(), "Templates");
            string filePath = Path.Combine(folder, $"{name}.cshtml");
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs, Encoding.Default);
            string mailText = sr.ReadToEnd();
            sr.Close();

            return mailText;
        }
    }
}