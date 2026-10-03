// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp6.Helpers;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1117ParametersMustBeOnSameLineOrSeparateLines>;

    public partial class SA1117CSharp9UnitTests
    {
        [Fact]
        public async Task TestValidTargetTypedNewExpressionAsync()
        {
            var testCode = @"
class Foo
{
    public Foo(int a, int b, int c)
    {
    }

    public void Method()
    {
        Foo x = new(
            1,
            2,
            3);
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInvalidTargetTypedNewExpressionAsync()
        {
            var testCode = @"
class Foo
{
    public Foo(int a, int b, int c)
    {
    }

    public void Method()
    {
        Foo x = new(1,
            2, 3);
    }
}";

            DiagnosticResult expected = Diagnostic().WithLocation(11, 16);
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestValidPrimaryConstructorAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo(
    int a,
    int b,
    int c)
{{
}}

{typeKeyword} Bar(int a, int b, int c)
{{
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestInvalidPrimaryConstructorAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo(int a, int b,
    {{|#0:int c|}})
{{
}}";

            var expected = this.GetExpectedResultTestInvalidPrimaryConstructor();
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.ReferenceTypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestValidPrimaryConstructorBaseListAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo(int a, int b, int c)
{{
}}

{typeKeyword} Bar(int a, int b, int c) : Foo(
    a,
    b,
    c)
{{
}}

{typeKeyword} Baz(int a, int b, int c) : Foo(a, b, c)
{{
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.ReferenceTypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestInvalidPrimaryConstructorBaseListAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo(int a, int b, int c)
{{
}}

{typeKeyword} Bar(int a, int b, int c) : Foo(a, b,
    {{|#0:c|}})
{{
}}";

            var expected = this.GetExpectedResultTestInvalidPrimaryConstructorBaseList();
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestInvalidPrimaryConstructor()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/53136
                Diagnostic().WithLocation(0),
                Diagnostic().WithLocation(0),
            };
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestInvalidPrimaryConstructorBaseList()
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
