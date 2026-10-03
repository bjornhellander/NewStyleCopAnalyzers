// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.DocumentationRules
{
    using Microsoft.CodeAnalysis.Testing;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.DocumentationRules.GenericTypeParameterDocumentationAnalyzer>;

    public partial class SA1620CSharp11UnitTests
    {
        protected override DiagnosticResult[] GetExpectedResultTestGenericTypeWithPrimaryConstructorWithWrongTypeParameterName()
        {
            return new[]
            {
                Diagnostic(StyleCop.Analyzers.DocumentationRules.GenericTypeParameterDocumentationAnalyzer.SA1620MissingTypeParameterDescriptor).WithLocation(0).WithArguments("U"),
            };
        }
    }
}
