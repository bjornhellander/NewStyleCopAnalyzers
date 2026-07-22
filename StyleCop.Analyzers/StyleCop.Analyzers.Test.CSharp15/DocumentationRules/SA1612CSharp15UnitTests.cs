// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.DocumentationRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1612ElementParameterDocumentationMustMatchElementParameters>;

    public partial class SA1612CSharp15UnitTests : SA1612CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionMethodParameterDocumentationOutOfOrderAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// A summary.
    /// </summary>
    /// <param name=""{|#0:second|}"">The second value.</param>
    /// <param name=""{|#1:first|}"">The first value.</param>
    public static void TestMethod(int first, int second)
    {
    }
}
";

            // TODO: Report bug - The compiler calls the registered method declaration action three times
            var expectedDiagnostic1 = Diagnostic().WithMessageFormat("The parameter documentation for '{0}' should be at position {1}").WithLocation(0).WithArguments("second", "2");
            var expectedDiagnostic2 = Diagnostic().WithMessageFormat("The parameter documentation for '{0}' should be at position {1}").WithLocation(1).WithArguments("first", "1");
            var expected = new[]
            {
                expectedDiagnostic1, expectedDiagnostic1, expectedDiagnostic1,
                expectedDiagnostic2, expectedDiagnostic2, expectedDiagnostic2,
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionMethodParameterDocumentationInOrderAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// A summary.
    /// </summary>
    /// <param name=""first"">The first value.</param>
    /// <param name=""second"">The second value.</param>
    public static void TestMethod(int first, int second)
    {
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
