﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.ReadabilityRules
{
    using Microsoft.CodeAnalysis;
    using StyleCop.Analyzers.ReadabilityRules;
    using Xunit;

    /// <summary>
    /// This class contains unit tests for <see cref="SA1126PrefixCallsCorrectly"/>.
    /// </summary>
    public class SA1126UnitTests
    {
        [Fact]
        public void TestDisabledByDefaultAndNotConfigurable()
        {
            var analyzer = new SA1126PrefixCallsCorrectly();
            Assert.Single(analyzer.SupportedDiagnostics);
            Assert.False(analyzer.SupportedDiagnostics[0].IsEnabledByDefault);
            Assert.Contains(WellKnownDiagnosticTags.NotConfigurable, analyzer.SupportedDiagnostics[0].CustomTags);
        }
    }
}
