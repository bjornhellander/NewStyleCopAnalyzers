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
        StyleCop.Analyzers.ReadabilityRules.SA1114ParameterListMustFollowDeclaration>;

    public partial class SA1114CSharp9UnitTests
    {
        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestPrimaryConstructorParametersList2LinesAfterOpeningParenthesisAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo(

    {{|#0:int a|}})
{{
}}";

            var expected = this.GetExpectedResultTestPrimaryConstructorParametersList2LinesAfterOpeningParenthesis();
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestPrimaryConstructorParametersListOnNextLineAsOpeningParenthesisAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo(
    int a)
{{
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.ReferenceTypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestPrimaryConstructorBaseListArgumentsList2LinesAfterOpeningParenthesisAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo(int a)
{{
}}

{typeKeyword} Bar(int a) : Foo(

    {{|#0:a|}})
{{
}}";

            var expected = this.GetExpectedResultTestPrimaryConstructorBaseListArgumentsList2LinesAfterOpeningParenthesis();
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.ReferenceTypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestPrimaryConstructorBaseListArgumentsListOnNextLineAsOpeningParenthesisAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo(int a)
{{
}}

{typeKeyword} Bar(int a) : Foo(
    a)
{{
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestPrimaryConstructorParametersList2LinesAfterOpeningParenthesis()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/53136
                Diagnostic().WithLocation(0),
                Diagnostic().WithLocation(0),
            };
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestPrimaryConstructorBaseListArgumentsList2LinesAfterOpeningParenthesis()
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
