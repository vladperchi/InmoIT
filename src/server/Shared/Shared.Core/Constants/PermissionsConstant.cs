// --------------------------------------------------------------------------------------------------
// <copyright file="PermissionsConstant.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace InmoIT.Shared.Core.Constants
{
    public static class PermissionsConstant
    {
        [DisplayName("Owners")]
        [Description("Owners Permissions")]
        public static class Owners
        {
            public const string View = "Permissions.Owners.View";
            public const string ViewAll = "Permissions.Owners.ViewAll";
            public const string Register = "Permissions.Owners.Register";
            public const string Update = "Permissions.Owners.Update";
            public const string Remove = "Permissions.Owners.Remove";
            public const string Export = "Permissions.Owners.Export";
        }

        [DisplayName("Properties")]
        [Description("Properties Permissions")]
        public static class Properties
        {
            public const string View = "Permissions.Properties.View";
            public const string ViewAll = "Permissions.Properties.ViewAll";
            public const string Register = "Permissions.Properties.Register";
            public const string Update = "Permissions.Properties.Update";
            public const string Remove = "Permissions.Properties.Remove";
            public const string Export = "Permissions.Properties.Export";
        }

        [DisplayName("Images")]
        [Description("Images Permissions")]
        public static class Images
        {
            public const string View = "Permissions.Images.View";
            public const string ViewAll = "Permissions.Images.ViewAll";
            public const string Update = "Permissions.Images.Update";
            public const string Remove = "Permissions.Images.Remove";
        }

        [DisplayName("Traces")]
        [Description("Traces Permissions")]
        public static class Traces
        {
            public const string ViewAll = "Permissions.Traces.ViewAll";
            public const string Export = "Permissions.Traces.Export";
        }

        [DisplayName("Documents")]
        [Description("Documents Permissions")]
        public static class Documents
        {
            public const string View = "Permissions.Documents.View";
            public const string ViewAll = "Permissions.Documents.ViewAll";
            public const string Create = "Permissions.Documents.Create";
            public const string Update = "Permissions.Documents.Update";
            public const string Remove = "Permissions.Documents.Remove";
        }

        [DisplayName("DocumentTypes")]
        [Description("DocumentTypes Permissions")]
        public static class DocumentTypes
        {
            public const string View = "Permissions.DocumentTypes.View";
            public const string ViewAll = "Permissions.DocumentTypes.ViewAll";
            public const string Create = "Permissions.DocumentTypes.Create";
            public const string Update = "Permissions.DocumentTypes.Update";
            public const string Remove = "Permissions.DocumentTypes.Remove";
        }

        [DisplayName("Customers")]
        [Description("Customers Permissions")]
        public static class Customers
        {
            public const string View = "Permissions.Customers.View";
            public const string ViewAll = "Permissions.Customers.ViewAll";
            public const string Register = "Permissions.Customers.Register";
            public const string Update = "Permissions.Customers.Update";
            public const string Remove = "Permissions.Customers.Remove";
            public const string Export = "Permissions.Customers.Export";
        }

        [DisplayName("Carts")]
        [Description("Carts Permissions")]
        public static class Carts
        {
            public const string View = "Permissions.Carts.View";
            public const string ViewAll = "Permissions.Carts.ViewAll";
            public const string Create = "Permissions.Carts.Create";
            public const string Remove = "Permissions.Carts.Remove";
        }

        [DisplayName("CartItems")]
        [Description("Cart Items Permissions")]
        public static class CartItems
        {
            public const string View = "Permissions.CartItems.View";
            public const string ViewAll = "Permissions.CartItems.ViewAll";
            public const string Add = "Permissions.CartItems.Add";
            public const string Update = "Permissions.CartItems.Update";
            public const string Remove = "Permissions.CartItems.Remove";
        }

        [DisplayName("Sales")]
        [Description("Sales Permissions")]
        public static class Sales
        {
            public const string View = "Permissions.Sales.View";
            public const string ViewAll = "Permissions.Sales.ViewAll";
            public const string Register = "Permissions.Sales.Register";
            public const string Update = "Permissions.Sales.Update";
            public const string Remove = "Permissions.Sales.Remove";
        }

        [DisplayName("Users")]
        [Description("Users Permissions")]
        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string ViewAll = "Permissions.Users.ViewAll";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Remove = "Permissions.Users.Remove";
            public const string Export = "Permissions.Users.Export";
        }

        [DisplayName("Roles")]
        [Description("Roles Permissions")]
        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
        }

        [DisplayName("RoleClaims")]
        [Description("Role Claims Permissions")]
        public static class RoleClaims
        {
            public const string View = "Permissions.RoleClaims.View";
            public const string Create = "Permissions.RoleClaims.Create";
            public const string Edit = "Permissions.RoleClaims.Edit";
            public const string Delete = "Permissions.RoleClaims.Delete";
        }

        [DisplayName("Logs")]
        [Description("Logs Permissions")]
        public static class Logs
        {
            public const string ViewAll = "Permissions.Logs.ViewAll";
            public const string Create = "Permissions.Logs.Create";
        }

        [DisplayName("Dashboards")]
        [Description("Dashboards Permissions")]
        public static class Dashboards
        {
            public const string View = "Permissions.Dashboards.View";
        }

        [DisplayName("Hangfire")]
        [Description("Hangfire Permissions")]
        public static class Hangfire
        {
            public const string View = "Permissions.Hangfire.View";
        }
    }
}