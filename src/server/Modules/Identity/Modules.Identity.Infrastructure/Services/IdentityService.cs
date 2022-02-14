// --------------------------------------------------------------------------------------------------
// <copyright file="IdentityService.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using InmoIT.Modules.Identity.Core.Abstractions;
using InmoIT.Modules.Identity.Core.Entities;
using InmoIT.Modules.Identity.Core.Exceptions;
using InmoIT.Modules.Identity.Core.Features.Users.Events;
using InmoIT.Shared.Core.Common.Enums;
using InmoIT.Shared.Core.Constants;
using InmoIT.Shared.Core.Interfaces.Services;
using InmoIT.Shared.Core.Settings;
using InmoIT.Shared.Core.Wrapper;
using InmoIT.Shared.Dtos.Identity.Users;
using InmoIT.Shared.Dtos.Mails;
using InmoIT.Shared.Dtos.Messages;
using InmoIT.Shared.Dtos.Upload;
using InmoIT.Shared.Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace InmoIT.Modules.Identity.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<InmoUser> _userManager;
        private readonly IJobService _jobService;
        private readonly IMailService _mailService;
        private readonly MailSettings _mailSettings;
        private readonly ISmsTwilioService _smsTwilioService;
        private readonly SmsTwilioSettings _smsTwilioSettings;
        private readonly IStringLocalizer<IdentityService> _localizer;
        private readonly IUploadService _uploadService;

        public IdentityService(
            UserManager<InmoUser> userManager,
            IUploadService uploadService,
            IJobService jobService,
            IMailService mailService,
            IOptions<MailSettings> mailSettings,
            ISmsTwilioService smsTwilioService,
            IOptions<SmsTwilioSettings> smsTwilioSettings,
            IStringLocalizer<IdentityService> localizer)
        {
            _userManager = userManager;
            _uploadService = uploadService;
            _jobService = jobService;
            _mailService = mailService;
            _mailSettings = mailSettings.Value;
            _smsTwilioSettings = smsTwilioSettings.Value;
            _smsTwilioService = smsTwilioService;
            _localizer = localizer;
        }

        public async Task<IResult> RegisterAsync(RegisterRequest request, string origin)
        {
            var userWithUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithUserName != null)
            {
                throw new IdentityException(string.Format(_localizer["Username {0} is in use."], request.UserName));
            }

            var user = new InmoUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                IsActive = true
            };

            if (request.EmailConfirmed) user.EmailConfirmed = true;
            if (request.PhoneNumberConfirmed) user.PhoneNumberConfirmed = true;

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                var userWithPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
                if (userWithPhoneNumber != null)
                {
                    throw new IdentityException(string.Format(_localizer["Phone number {0} is registered."], request.PhoneNumber));
                }
            }

            var userWithEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithEmail == null)
            {
                user.AddDomainEvent(new UserRegisteredEvent(user));
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RolesConstant.Staff);
                    if (!_mailSettings.EnableVerification && !_smsTwilioSettings.EnableVerification)
                    {
                        return await Result<string>.SuccessAsync(user.Id, message: string.Format(_localizer["User {0} Registered."], user.UserName));
                    }

                    var messages = new List<string> { string.Format(_localizer["User {0} Registered."], user.UserName) };
                    if (_mailSettings.EnableVerification)
                    {
                        string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
                        var mailRequest = new MailRequest
                        {
                            To = user.Email,
                            Subject = _localizer["Confirm Registration"],
                            Body = string.Format(_localizer["Please confirm your InmoIT account by <a href='{0}'>clicking here</a>."], emailVerificationUri)
                        };
                        _jobService.Enqueue(() => _mailService.SendAsync(mailRequest));
                        messages.Add(_localizer["Please check your Email to verify"]);
                    }

                    if (_smsTwilioSettings.EnableVerification)
                    {
                        string mobilePhoneVerificationCode = await GetPhoneVerificationCodeAsync(user);
                        var smsTwilioRequest = new SmsTwilioRequest
                        {
                            Number = user.PhoneNumber,
                            Message = string.Format(_localizer["Please confirm your InmoIT account by this code: {0}"], mobilePhoneVerificationCode)
                        };
                        _jobService.Enqueue(() => _smsTwilioService.SendAsync(smsTwilioRequest));
                        messages.Add(_localizer["Please check your Phone for code in SMS to verify!"]);
                    }

                    return await Result<string>.SuccessAsync(user.Id, messages: messages);
                }
                else
                {
                    var errorMessages = result.Errors.Select(a => _localizer[a.Description].ToString()).Distinct().ToList();
                    throw new IdentityCustomException(_localizer, errorMessages);
                }
            }
            else
            {
                throw new IdentityException(string.Format(_localizer["Email {0} is registered."], request.Email));
            }
        }

        public async Task<IResult<string>> GetUserPictureAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return await Result<string>.FailAsync(_localizer["User Not Found"]);
            }

            return await Result<string>.SuccessAsync(data: user.ImageUrl);
        }

        public async Task<IResult<string>> UpdateUserPictureAsync(UpdateUserPictureRequest request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return await Result<string>.FailAsync(message: _localizer["User Not Found"]);
            }

            if (request != null)
            {
                var fileUploadRequest = new FileUploadRequest
                {
                    Data = request?.Data,
                    Extension = Path.GetExtension(request.FileName).ToLower(),
                    UploadStorageType = UploadStorageType.Staff
                };
                string fileName = await GenerateFileName(20);
                fileUploadRequest.FileName = fileName + fileUploadRequest.Extension;
                user.ImageUrl = await _uploadService.UploadAsync(fileUploadRequest, FileType.Image);
            }

            string filePath = user.ImageUrl;
            var identityResult = await _userManager.UpdateAsync(user);
            var errors = identityResult.Errors.Select(e => _localizer[e.Description].ToString()).ToList();
            return identityResult.Succeeded ? await Result<string>.SuccessAsync(data: filePath) : await Result<string>.FailAsync(errors);
        }

        private async Task<string> GetEmailVerificationUriAsync(InmoUser user, string origin)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string route = "api/v1/identity/confirm-email/";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);
            return QueryHelpers.AddQueryString(verificationUri, "code", code);
        }

        private async Task<string> GetPhoneVerificationCodeAsync(InmoUser user)
        {
            return await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
        }

        private async Task<string> GenerateFileName(int length)
        {
            return await Utilities.GenerateCode("S", length);
        }

        public async Task<IResult<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(_localizer);
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                if (user.PhoneNumberConfirmed || !_smsTwilioSettings.EnableVerification)
                {
                    return await Result<string>.SuccessAsync(user.Id, string.Format(_localizer["Account Email {0} Confirmed. Use the /api/v1/identity/token endpoint to generate JWT."], user.Email));
                }
                else
                {
                    return await Result<string>.SuccessAsync(user.Id, string.Format(_localizer["Account Email {0} Confirmed. Confirm your phone number before using the /api/v1/identity/token endpoint to generate JWT."], user.Email));
                }
            }
            else
            {
                throw new IdentityException(string.Format(_localizer["An error occurred while confirming {0}"], user.Email));
            }
        }

        public async Task<IResult<string>> ConfirmPhoneNumberAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(_localizer);
            }

            var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, code);
            if (result.Succeeded)
            {
                if (user.EmailConfirmed)
                {
                    return await Result<string>.SuccessAsync(user.Id, string.Format(_localizer["Account Confirmed for Phone Number {0}. Use the /api/v1/identity/token endpoint to generate JWT."], user.PhoneNumber));
                }
                else
                {
                    return await Result<string>.SuccessAsync(user.Id, string.Format(_localizer["Account Confirmed for Phone Number {0}. Confirm your E-mail before using the /api/v1/identity/token endpoint to generate JWT."], user.PhoneNumber));
                }
            }
            else
            {
                throw new IdentityException(string.Format(_localizer["An error occurred while confirming {0}"], user.PhoneNumber));
            }
        }

        public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new UserNotFoundException(_localizer);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                throw new IdentityCustomException(_localizer, null);
            }

            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string route = "api/v1/identity/reset-password";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            string passwordResetUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
            var mailRequest = new MailRequest
            {
                To = request.Email,
                Subject = _localizer["Reset Password"],
                Body = string.Format(_localizer["Please reset your password by <a href='{0}>clicking here</a>."], HtmlEncoder.Default.Encode(passwordResetUrl))
            };
            _jobService.Enqueue(() => _mailService.SendAsync(mailRequest));
            return await Result.SuccessAsync(_localizer["Password Reset Mail has been sent to your authorized Email."]);
        }

        public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new UserNotFoundException(_localizer);
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (result.Succeeded)
            {
                return await Result.SuccessAsync(_localizer["Password Reset Successful!"]);
            }
            else
            {
                return await Result.FailAsync(_localizer["An error occurred while password reset."]);
            }
        }
    }
}