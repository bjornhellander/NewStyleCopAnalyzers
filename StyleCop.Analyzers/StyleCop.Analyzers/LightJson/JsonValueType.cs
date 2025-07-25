﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace LightJson
{
    /// <summary>
    /// Enumerates the types of Json values.
    /// </summary>
    internal enum JsonValueType : byte
    {
        /// <summary>
        /// A null value.
        /// </summary>
        Null = 0,

        /// <summary>
        /// A boolean value.
        /// </summary>
        Boolean,

        /// <summary>
        /// A number value.
        /// </summary>
        Number,

        /// <summary>
        /// A string value.
        /// </summary>
        String,

        /// <summary>
        /// An object value.
        /// </summary>
        Object,

        /// <summary>
        /// An array value.
        /// </summary>
        Array,
    }
}
