// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.SA1015ClosingGenericBracketsMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1015ClosingGenericBracketsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1015CSharp9UnitTests
    {
        [Fact]
        public async Task TestFunctionPointerAsync()
        {
            var testCode = @"public unsafe class TestClass
{
    private delegate*<int, void {|#0:>|} a;
    private delegate*<int, void{|#1:>|}b;
    private delegate*<delegate*<int>, void> c;

    public void TestMethod(delegate*<int, void> p)
    {
        var e = (delegate*<int, void>)null;
    }
}
";

            var fixedCode = @"public unsafe class TestClass
{
    private delegate*<int, void> a;
    private delegate*<int, void> b;
    private delegate*<delegate*<int>, void> c;

    public void TestMethod(delegate*<int, void> p)
    {
        var e = (delegate*<int, void>)null;
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic(DescriptorNotPreceded).WithLocation(0),
                Diagnostic(DescriptorFollowed).WithLocation(1),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
