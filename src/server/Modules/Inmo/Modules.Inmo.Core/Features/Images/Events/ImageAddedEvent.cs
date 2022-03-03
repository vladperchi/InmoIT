// --------------------------------------------------------------------------------------------------
// <copyright file="ImageAddedEvent.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Domain;
using InmoIT.Shared.Dtos.Inmo.Images;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InmoIT.Modules.Inmo.Core.Features.Images.Events
{
    public class ImageAddedEvent : Event
    {
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }

        public List<PropertyImageRequest> PropertyImageList { get; set; }

        public ImageAddedEvent(PropertyImage propertyImage)
        {
            Id = propertyImage.Id;
            PropertyId = propertyImage.PropertyId;
            AggregateId = propertyImage.PropertyId;
            RelatedEntities = new[] { typeof(PropertyImage) };
            EventDescription = $"Added Image:{PropertyImageList.ToArray()}:::PropId:{propertyImage.PropertyId}:::Id:{Id}";
        }
    }
}