// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.DocumentationRules
{
    using Microsoft.CodeAnalysis.Testing;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1604ElementDocumentationMustHaveSummary>;

    public partial class SA1604CSharp11UnitTests
    {
        protected override DiagnosticResult[] GetExpectedResultTestTypeWithPrimaryConstructorWithoutSummary()
        {
            return new[]
            {
                Diagnostic().WithLocation(0),
            };
        }
    }
}
