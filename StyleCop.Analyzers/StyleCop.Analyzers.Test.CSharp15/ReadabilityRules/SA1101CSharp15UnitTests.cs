// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1101PrefixLocalCallsWithThis,
        StyleCop.Analyzers.ReadabilityRules.SA1101CodeFixProvider>;

    public partial class SA1101CSharp15UnitTests
    {
        [Fact]
        public async Task TestExtensionBlockDeclarationIndexerAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension(string source)
    {
        public char this[int index] => source[index];
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [InlineData("MyConst")]
        [InlineData("MyStaticField")]
        [InlineData("MyStaticProp")]
        [InlineData("MyStaticFunc()")]
        public async Task TestUnionReferencingOwnMemberWithoutDignosticAsync(string expr)
        {
            var testCode = $@"
public union TestUnion(string, int)
{{
    private const int MyConst = 42;
    private static int MyStaticField = 42;
    private static int MyStaticProp => 42;
    private static int MyStaticFunc() => 42;

    public void TestMethod()
    {{
        _ = {expr};
    }}
}}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [InlineData("Value")]
        [InlineData("MyInstanceProp")]
        [InlineData("MyInstanceFunc", "()")]
        public async Task TestUnionReferencingOwnMemberWithDiagnosticAsync(string name, string suffix = "")
        {
            var testCode = $@"
public union TestUnion(string, int)
{{
    private int MyInstanceProp => 42;
    private int MyInstanceFunc() => 42;

    public void TestMethod()
    {{
        _ = [|{name}|]{suffix};
    }}
}}
";

            var fixedCode = $@"
public union TestUnion(string, int)
{{
    private int MyInstanceProp => 42;
    private int MyInstanceFunc() => 42;

    public void TestMethod()
    {{
        _ = this.{name}{suffix};
    }}
}}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [InlineData("break")]
        [InlineData("continue")]
        public async Task TestLabeledBreakOrContinueDoesNotRequireThisAsync(string keyword)
        {
            // The label named by a labeled 'break'/'continue' shares a declaration space with labels only, so it
            // must not be confused with the instance field of the same name below.
            var testCode = $@"
public class TestClass
{{
    private int outer;

    public void TestMethod()
    {{
        outer: while (true)
        {{
            {keyword} outer;
        }}
    }}
}}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
