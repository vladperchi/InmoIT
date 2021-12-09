﻿// --------------------------------------------------------------------------------------------------
// <copyright file="PersistenceSettings.cs" company="InmoIT">
// Copyright (c) InmoIT. All rights reserved.
// Developer: Vladimir P. CHibás (vladperchi).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace InmoIT.Shared.Core.Settings
{
    public class PersistenceSettings
    {
        public bool UseMsSql { get; set; }

        // Declare other database providers example Postgres, MySql

        public PersistenceConnectionStrings ConnectionStrings { get; set; }

        public class PersistenceConnectionStrings
        {
            public string MSSQL { get; set; }
        }
    }
}