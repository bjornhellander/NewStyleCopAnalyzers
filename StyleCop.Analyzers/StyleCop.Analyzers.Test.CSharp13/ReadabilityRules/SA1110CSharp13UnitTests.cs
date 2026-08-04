// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.ReadabilityRules
{
    using Microsoft.CodeAnalysis.Testing;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1110OpeningParenthesisMustBeOnDeclarationLine,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1110CSharp13UnitTests
    {
        protected override DiagnosticResult[] GetExpectedResultTestPrimaryConstructorBaseList()
        {
            return new[]
            {
                // Diagnostic previously issued twice because of https://github.com/dotnet/roslyn/issues/70488
                Diagnostic().WithLocation(0),
            };
        }
    }
}
