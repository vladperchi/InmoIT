// --------------------------------------------------------------------------------------------------
// <copyright file="ImageFilterSpecification.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Interfaces.Specifications;
using Microsoft.EntityFrameworkCore;

namespace InmoIT.Modules.Inmo.Core.Specifications
{
    public class ImageFilterSpecification : Specification<PropertyImage>
    {
        public ImageFilterSpecification(string searchString)
        {
            Includes.Add(x => x.Property);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = x => !string.IsNullOrWhiteSpace(x.CodeImage) && x.Enabled
                && (EF.Functions.Like(x.Caption.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.CodeImage.ToLower(), $"%{searchString.ToLower()}%")
                || EF.Functions.Like(x.Property.Name.ToLower(), $"%{searchString.ToLower()}%"));
            }
            else
            {
                Criteria = x => !string.IsNullOrWhiteSpace(x.CodeImage) && x.Enabled;
            }
        }
    }
}