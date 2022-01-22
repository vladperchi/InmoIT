// --------------------------------------------------------------------------------------------------
// <copyright file="ImageAddedEvent.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System;

using InmoIT.Modules.Inmo.Core.Entities;
using InmoIT.Shared.Core.Domain;

namespace InmoIT.Modules.Inmo.Core.Features.Images.Events
{
    public class ImageAddedEvent : Event
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; }

        public string Caption { get; set; }

        public bool Enabled { get; set; }

        public string CodeImage { get; set; }

        public Guid PropertyId { get; set; }

        public ImageAddedEvent(PropertyImage propertyImage)
        {
            Id = propertyImage.Id;
            ImageUrl = propertyImage.ImageUrl;
            Caption = propertyImage.Caption;
            Enabled = propertyImage.Enabled;
            CodeImage = propertyImage.CodeImage;
            PropertyId = propertyImage.PropertyId;
            AggregateId = propertyImage.Id;
            RelatedEntities = new[] { typeof(PropertyImage) };
            EventDescription = $"Added Image to PropertyId: {propertyImage.PropertyId}.";
        }
    }
}