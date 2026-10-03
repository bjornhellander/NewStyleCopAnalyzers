// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp6.Helpers;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1505OpeningBracesMustNotBeFollowedByBlankLine,
        StyleCop.Analyzers.LayoutRules.SA1505CodeFixProvider>;

    public partial class SA1505CSharp9UnitTests
    {
        [Fact]
        public async Task TestSingleLineRecordAsync()
        {
            var testCode = @"namespace TestNamespace
{
    public record TestRecord;
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, testCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestBlankLineAfterOpeningBraceInTypeWithPrimaryConstructorAsync(string typeKeyword)
        {
            var testCode = $@"public {typeKeyword} TestType(int X)
{{|#0:{{|}}

    public int Property => 0;
}}";

            var fixedCode = $@"public {typeKeyword} TestType(int X)
{{
    public int Property => 0;
}}";

            var expected = this.GetExpectedResultTestBlankLineAfterOpeningBraceInTypeWithPrimaryConstructor();
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestBlankLineAfterOpeningBraceInTypeWithPrimaryConstructor()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/53136
                Diagnostic().WithLocation(0),
                Diagnostic().WithLocation(0),
            };
        }
    }
}
