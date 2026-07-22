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
        StyleCop.Analyzers.DocumentationRules.SA1625ElementDocumentationMustNotBeCopiedAndPasted>;

    public partial class SA1625CSharp15UnitTests : SA1625CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionMethodWithDuplicatedSummaryAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// Some documentation.
    /// </summary>
    /// {|#0:<summary>
    /// Some documentation.
    /// </summary>|}
    public static void TestMethod()
    {
    }
}
";

            // TODO: Report bug - The compiler calls the registered method declaration action three times
            var expectedDiagnostic = Diagnostic().WithLocation(0);
            var expected = new[] { expectedDiagnostic, expectedDiagnostic, expectedDiagnostic };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionMethodWithoutDuplicatedSummaryAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// Some documentation.
    /// </summary>
    public static void TestMethod()
    {
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
