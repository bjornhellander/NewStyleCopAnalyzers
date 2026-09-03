// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.MaintainabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1119StatementMustNotUseUnnecessaryParenthesis,
        StyleCop.Analyzers.MaintainabilityRules.SA1119CodeFixProvider>;

    public partial class SA1119CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionMethodWithUnnecessaryParenthesisAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        int x = (5 + 3);
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        int x = 5 + 3;
    }
}
";

            DiagnosticResult mainDiagnostic = Diagnostic(SA1119StatementMustNotUseUnnecessaryParenthesis.DiagnosticId).WithSpan(6, 17, 6, 24);
            DiagnosticResult openParenDiagnostic = Diagnostic(SA1119StatementMustNotUseUnnecessaryParenthesis.ParenthesesDiagnosticId).WithLocation(6, 17);
            DiagnosticResult closeParenDiagnostic = Diagnostic(SA1119StatementMustNotUseUnnecessaryParenthesis.ParenthesesDiagnosticId).WithLocation(6, 23);
            var expected = new[] { mainDiagnostic, openParenDiagnostic, closeParenDiagnostic };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnsafeExpressionWithUnnecessaryParenthesisAsync()
        {
            var testCode = @"
public class TestClass
{
    public static int TestMethod()
    {
        return unsafe((1 + 1));
    }
}
";

            var fixedCode = @"
public class TestClass
{
    public static int TestMethod()
    {
        return unsafe(1 + 1);
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic(SA1119StatementMustNotUseUnnecessaryParenthesis.DiagnosticId).WithSpan(6, 23, 6, 30),
                Diagnostic(SA1119StatementMustNotUseUnnecessaryParenthesis.ParenthesesDiagnosticId).WithLocation(6, 23),
                Diagnostic(SA1119StatementMustNotUseUnnecessaryParenthesis.ParenthesesDiagnosticId).WithLocation(6, 29),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
