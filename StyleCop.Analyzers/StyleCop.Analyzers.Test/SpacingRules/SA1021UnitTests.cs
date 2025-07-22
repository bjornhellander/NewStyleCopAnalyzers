// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.SpacingRules
{
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.SpacingRules;

    public class SA1021UnitTests : NumberSignSpacingTestBase
    {
        protected override string Sign
        {
            get
            {
                return "-";
            }
        }

        protected override DiagnosticAnalyzer Analyzer => new SA1021NegativeSignsMustBeSpacedCorrectly();

        protected override CodeFixProvider CodeFix => new TokenSpacingCodeFixProvider();
    }
}
