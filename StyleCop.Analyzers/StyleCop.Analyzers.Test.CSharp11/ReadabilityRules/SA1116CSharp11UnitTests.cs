// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.ReadabilityRules
{
    using Microsoft.CodeAnalysis.Testing;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1116SplitParametersMustStartOnLineAfterDeclaration,
        StyleCop.Analyzers.ReadabilityRules.SA1116CodeFixProvider>;

    public partial class SA1116CSharp11UnitTests
    {
        protected override DiagnosticResult[] GetExpectedResultTestPrimaryConstructorSplitParametersNotStartingOnNextLine()
        {
            return new[]
            {
                Diagnostic().WithLocation(0),
            };
        }
    }
}
