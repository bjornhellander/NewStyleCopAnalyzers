// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp6.Helpers;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1116SplitParametersMustStartOnLineAfterDeclaration,
        StyleCop.Analyzers.ReadabilityRules.SA1116CodeFixProvider>;

    public partial class SA1116CSharp9UnitTests
    {
        [Fact]
        public async Task TestTargetTypedNewExpressionnAsync()
        {
            var testCode = @"
class Foo
{
    public Foo(int a, int b)
    {
    }

    public void Method()
    {
        Foo x = new(1,
            2);
    }
}";

            var fixedCode = @"
class Foo
{
    public Foo(int a, int b)
    {
    }

    public void Method()
    {
        Foo x = new(
            1,
            2);
    }
}";

            DiagnosticResult expected = Diagnostic().WithLocation(10, 21);
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestPrimaryConstructorSplitParametersNotStartingOnNextLineAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo({{|#0:int a|}},
    int b)
{{
}}";

            var fixedCode = $@"
{typeKeyword} Foo(
    int a,
    int b)
{{
}}";

            var expected = this.GetExpectedResultTestPrimaryConstructorSplitParametersNotStartingOnNextLine();
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.ReferenceTypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestPrimaryConstructorBaseListSplitArgumentsNotStartingOnNextLineAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo(int a, int b)
{{
}}

{typeKeyword} Bar(int a, int b) : Foo({{|#0:a|}},
    b)
{{
}}";

            var fixedCode = $@"
{typeKeyword} Foo(int a, int b)
{{
}}

{typeKeyword} Bar(int a, int b) : Foo(
    a,
    b)
{{
}}";

            var expected = this.GetExpectedResultTestPrimaryConstructorBaseListSplitArgumentsNotStartingOnNextLine();
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestPrimaryConstructorSplitParametersNotStartingOnNextLine()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/53136
                Diagnostic().WithLocation(0),
                Diagnostic().WithLocation(0),
            };
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestPrimaryConstructorBaseListSplitArgumentsNotStartingOnNextLine()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/70488
                Diagnostic().WithLocation(0),
                Diagnostic().WithLocation(0),
            };
        }
    }
}
