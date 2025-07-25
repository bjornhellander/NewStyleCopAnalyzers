﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace LightJson.Serialization
{
    /// <summary>
    /// Represents a position within a plain text resource.
    /// </summary>
    internal struct TextPosition
    {
        /// <summary>
        /// The column position, 0-based.
        /// </summary>
        public long Column;

        /// <summary>
        /// The line position, 0-based.
        /// </summary>
        public long Line;
    }
}
