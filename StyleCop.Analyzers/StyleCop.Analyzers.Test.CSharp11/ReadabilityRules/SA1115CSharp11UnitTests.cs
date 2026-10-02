// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.ReadabilityRules
{
    using Microsoft.CodeAnalysis.Testing;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1115ParameterMustFollowComma>;

    public partial class SA1115CSharp11UnitTests
    {
        protected override DiagnosticResult[] GetExpectedResultTestPrimaryConstructorEmptyLineBetweenParameters()
        {
            return new[]
            {
                Diagnostic().WithLocation(0),
            };
        }
    }
}
