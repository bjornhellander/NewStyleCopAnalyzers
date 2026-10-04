// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1014OpeningGenericBracketsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1014CSharp9UnitTests
    {
        [Fact]
        public async Task TestFunctionPointerAsync()
        {
            var testCode = @"public unsafe class TestClass
{
    private delegate*{|#0:<|} int, void> a;
    private delegate* unmanaged[Cdecl]{|#1:<|} int, void> b;
    private delegate* managed {|#2:<|}int, void> c;
    private delegate*<delegate*<int>, void> d;

    public void TestMethod()
    {
        var e = (delegate*<int, void>)null;
    }
}
";

            var fixedCode = @"public unsafe class TestClass
{
    private delegate*<int, void> a;
    private delegate* unmanaged[Cdecl]<int, void> b;
    private delegate* managed<int, void> c;
    private delegate*<delegate*<int>, void> d;

    public void TestMethod()
    {
        var e = (delegate*<int, void>)null;
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(0).WithArguments("followed"),
                Diagnostic().WithLocation(1).WithArguments("followed"),
                Diagnostic().WithLocation(2).WithArguments("preceded"),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a space between the asterisk of a function pointer and the opening bracket is not reported,
        /// since it is reported by SA1023.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestFunctionPointerWithSpaceAfterAsteriskAsync()
        {
            var testCode = @"public unsafe class TestClass
{
    private delegate* <int, void> a;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
