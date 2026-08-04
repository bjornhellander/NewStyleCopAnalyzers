// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp12.ReadabilityRules
{
    using Microsoft.CodeAnalysis.Testing;

    public partial class SA1130CSharp12UnitTests
    {
        protected override DiagnosticResult[] GetCompilerExpectedResultCodeFixSpecialCases()
        {
            return new[]
            {
                DiagnosticResult.CompilerError("CS1065").WithLocation(12, 53),
                DiagnosticResult.CompilerError("CS7014").WithLocation(13, 47),
                DiagnosticResult.CompilerError("CS1670").WithLocation(14, 47),
                DiagnosticResult.CompilerError("CS1669").WithLocation(15, 42),
                DiagnosticResult.CompilerError("CS0225").WithLocation(14, 47),
            };
        }
    }
}
