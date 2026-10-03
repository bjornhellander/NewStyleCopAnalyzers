// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.DocumentationRules
{
    using Microsoft.CodeAnalysis.Testing;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1614ElementParameterDocumentationMustHaveText>;

    public partial class SA1614CSharp11UnitTests
    {
        protected override DiagnosticResult[] GetExpectedResultTestPrimaryConstructorParameterWithoutText()
        {
            return new[]
            {
                Diagnostic().WithLocation(0),
            };
        }
    }
}
