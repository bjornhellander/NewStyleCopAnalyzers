// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.OrderingRules
{
    using Microsoft.CodeAnalysis.Testing;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1205PartialElementsMustDeclareAccess,
        StyleCop.Analyzers.OrderingRules.SA1205CodeFixProvider>;

    public partial class SA1205CSharp11UnitTests
    {
        protected override DiagnosticResult[] GetExpectedResultTestPartialTypeWithPrimaryConstructorWithoutAccessModifier()
        {
            return new[]
            {
                Diagnostic().WithLocation(0),
            };
        }
    }
}
