// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1101PrefixLocalCallsWithThis,
        StyleCop.Analyzers.ReadabilityRules.SA1101CodeFixProvider>;

    public partial class SA1101CSharp15UnitTests : SA1101CSharp14UnitTests
    {
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
            // TODO: Report bug - The compiler calls the registered member access action three times
            var testCode = $@"
public union TestUnion(string, int)
{{
    private int MyInstanceProp => 42;
    private int MyInstanceFunc() => 42;

    public void TestMethod()
    {{
        _ = {{|#0:{name}|}}{suffix};
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

            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
