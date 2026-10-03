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
        StyleCop.Analyzers.ReadabilityRules.SA1113CommaMustBeOnSameLineAsPreviousParameter,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1113CSharp9UnitTests
    {
        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestPrimaryConstructorCommaPlacedAtTheSameLineAsTheSecondParameterAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo(int a
    {{|#0:,|}} int b)
{{
}}";

            var fixedCode = $@"
{typeKeyword} Foo(int a,
    int b)
{{
}}";

            var expected = this.GetExpectedResultTestPrimaryConstructorCommaPlacedAtTheSameLineAsTheSecondParameter();
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.ReferenceTypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestPrimaryConstructorBaseListCommaPlacedAtTheSameLineAsTheSecondArgumentAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo(int a, int b)
{{
}}

{typeKeyword} Bar(int a, int b) : Foo(a
    {{|#0:,|}} b)
{{
}}";

            var fixedCode = $@"
{typeKeyword} Foo(int a, int b)
{{
}}

{typeKeyword} Bar(int a, int b) : Foo(a,
    b)
{{
}}";

            var expected = this.GetExpectedResultTestPrimaryConstructorBaseListCommaPlacedAtTheSameLineAsTheSecondArgument();
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestPrimaryConstructorCommaPlacedAtTheSameLineAsTheSecondParameter()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/53136
                Diagnostic().WithLocation(0),
                Diagnostic().WithLocation(0),
            };
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestPrimaryConstructorBaseListCommaPlacedAtTheSameLineAsTheSecondArgument()
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
