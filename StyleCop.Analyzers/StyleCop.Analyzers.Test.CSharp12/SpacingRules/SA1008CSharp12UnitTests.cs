// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp12.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    using static StyleCop.Analyzers.SpacingRules.SA1008OpeningParenthesisMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1008OpeningParenthesisMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1008CSharp12UnitTests
    {
        [Fact]
        public async Task TestTupleUsingAliasAsync()
        {
            const string testCode = @"
using TestAlias ={|#0:(|}string X, bool Y);";

            const string fixedCode = @"
using TestAlias = (string X, bool Y);";

            var expected = Diagnostic(DescriptorPreceded).WithLocation(0);
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestParenthesizedLambdaInCollectionExpressionAsync()
        {
            var testCode = @"
class TestClass
{
    private System.Action[] actions = [ [|(|]) => {}];
}
";

            var fixedCode = @"
class TestClass
{
    private System.Action[] actions = [() => {}];
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestCollectionExpressionAsync()
        {
            var testCode = @"
namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod()
        {
            int[] x = [ {|#0:(|} 0 + 0)];
        }
    }
}
";

            var fixedCode = @"
namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod()
        {
            int[] x = [(0 + 0)];
        }
    }
}
";

            DiagnosticResult[] expectedResults =
            {
                Diagnostic(DescriptorNotPreceded).WithLocation(0),
                Diagnostic(DescriptorNotFollowed).WithLocation(0),
            };

            await VerifyCSharpFixAsync(
                testCode,
                expectedResults,
                fixedCode,
                CancellationToken.None).ConfigureAwait(true);
        }
    }
}
