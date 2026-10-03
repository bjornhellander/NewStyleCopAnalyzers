// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.DocumentationRules
{
    using Microsoft.CodeAnalysis.Testing;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1629DocumentationTextMustEndWithAPeriod,
        StyleCop.Analyzers.DocumentationRules.SA1629CodeFixProvider>;

    public partial class SA1629CSharp11UnitTests
    {
        protected override DiagnosticResult[] GetExpectedResultTestTypeWithPrimaryConstructorSummaryWithoutPeriod()
        {
            return new[]
            {
                Diagnostic().WithLocation(0),
            };
        }
    }
}
