﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.DocumentationRules
{
    using Microsoft.CodeAnalysis;
    using StyleCop.Analyzers.DocumentationRules;
    using Xunit;

    /// <summary>
    /// This class contains unit tests for <see cref="SA1632DocumentationTextMustMeetMinimumCharacterLength"/>.
    /// </summary>
    public class SA1632UnitTests
    {
        [Fact]
        public void TestDisabledByDefaultAndNotConfigurable()
        {
            var analyzer = new SA1632DocumentationTextMustMeetMinimumCharacterLength();
            Assert.Single(analyzer.SupportedDiagnostics);
            Assert.False(analyzer.SupportedDiagnostics[0].IsEnabledByDefault);
            Assert.Contains(WellKnownDiagnosticTags.NotConfigurable, analyzer.SupportedDiagnostics[0].CustomTags);
        }
    }
}
