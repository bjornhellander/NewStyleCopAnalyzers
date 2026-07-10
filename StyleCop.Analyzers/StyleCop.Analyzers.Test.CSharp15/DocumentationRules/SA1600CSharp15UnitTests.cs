// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.DocumentationRules;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1600ElementsMustBeDocumented,
        StyleCop.Analyzers.DocumentationRules.SA1600CodeFixProvider>;

    public partial class SA1600CSharp15UnitTests : SA1600CSharp14UnitTests
    {
        protected override DiagnosticResult[] GetExpectedResultTestRegressionMethodGlobalNamespace(string code)
        {
            if (code == "public void {|#0:TestMember|}() { }")
            {
                return new[]
                {
                    // /0/Test0.cs(4,1): error CS0106: The modifier 'public' is not valid for this item
                    DiagnosticResult.CompilerError("CS0106").WithSpan(4, 1, 4, 7).WithArguments("public"),

                    // /0/Test0.cs(4,1): error CS8805: Program using top-level statements must be an executable.
                    DiagnosticResult.CompilerError("CS8805").WithSpan(4, 1, 4, 29),
                };
            }

            if (code.StartsWith("public string {|#0:this"))
            {
                return new[]
                {
                    // /0/Test0.cs(4,15): error CS9348: A compilation unit cannot directly contain members such as fields, methods or properties
                    DiagnosticResult.CompilerError("CS9348").WithSpan(4, 15, 4, 19),

                    Diagnostic().WithLocation(0),
                };
            }

            if (code.StartsWith("public string"))
            {
                return new[]
                {
                    // /0/Test0.cs(4,15): error CS9348: A compilation unit cannot directly contain members such as fields, methods or properties
                    DiagnosticResult.CompilerError("CS9348").WithSpan(4, 15, 4, 25),

                    Diagnostic().WithLocation(0),
                };
            }

            return new[]
            {
                // /0/Test0.cs(4,27): error CS9348: A compilation unit cannot directly contain members such as fields, methods or properties
                DiagnosticResult.CompilerError("CS9348").WithSpan(4, 27, 4, 37),

                Diagnostic().WithLocation(0),
            };
        }
    }
}
