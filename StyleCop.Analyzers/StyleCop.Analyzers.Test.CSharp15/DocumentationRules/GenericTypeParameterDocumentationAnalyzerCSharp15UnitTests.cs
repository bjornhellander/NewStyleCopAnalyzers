// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.DocumentationRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.DocumentationRules.GenericTypeParameterDocumentationAnalyzer>;

    public class GenericTypeParameterDocumentationAnalyzerCSharp15UnitTests
    {
        [Fact]
        public async Task TestGenericUnionWithMissingTypeParamNameAsync()
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
///{|#0:<typeparam>Test</typeparam>|}
public union TestUnion<T>(string, int)
{
}
";

            var expected = Diagnostic(GenericTypeParameterDocumentationAnalyzer.SA1621Descriptor).WithLocation(0);

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestGenericUnionWithTypeParamDocumentationAsync()
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
/// <typeparam name=""T"">Test</typeparam>
public union TestUnion<T>(string, int)
{
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
