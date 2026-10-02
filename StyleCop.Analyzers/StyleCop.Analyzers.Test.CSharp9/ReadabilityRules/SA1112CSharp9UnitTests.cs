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
        StyleCop.Analyzers.ReadabilityRules.SA1112ClosingParenthesisMustBeOnLineOfOpeningParenthesis,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1112CSharp9UnitTests
    {
        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestPrimaryConstructorWithNoParametersClosingParenthesisOnTheNextLineAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo(
{{|#0:)|}}
{{
}}";

            var fixedCode = $@"
{typeKeyword} Foo()
{{
}}";

            var expected = this.GetExpectedResultTestPrimaryConstructorWithNoParametersClosingParenthesisOnTheNextLine();
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.ReferenceTypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestPrimaryConstructorBaseListWithNoArgumentsClosingParenthesisOnTheNextLineAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo()
{{
}}

{typeKeyword} Bar(int x) : Foo(
{{|#0:)|}}
{{
}}";

            var fixedCode = $@"
{typeKeyword} Foo()
{{
}}

{typeKeyword} Bar(int x) : Foo()
{{
}}";

            var expected = this.GetExpectedResultTestPrimaryConstructorBaseListWithNoArgumentsClosingParenthesisOnTheNextLine();
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestPrimaryConstructorWithNoParametersClosingParenthesisOnTheNextLine()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/53136
                Diagnostic().WithLocation(0),
                Diagnostic().WithLocation(0),
            };
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestPrimaryConstructorBaseListWithNoArgumentsClosingParenthesisOnTheNextLine()
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
