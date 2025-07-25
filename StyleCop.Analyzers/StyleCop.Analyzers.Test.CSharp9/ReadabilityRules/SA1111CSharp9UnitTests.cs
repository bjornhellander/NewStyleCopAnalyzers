﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp8.ReadabilityRules;
    using StyleCop.Analyzers.Test.Helpers;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1111ClosingParenthesisMustBeOnLineOfLastParameter,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1111CSharp9UnitTests : SA1111CSharp8UnitTests
    {
        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        [WorkItem(3785, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3785")]
        public async Task TestPrimaryConstructorWithParameterAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo(int x
    {{|#0:)|}}
{{
}}";

            var fixedCode = $@"
{typeKeyword} Foo(int x)
{{
}}";

            var expected = this.GetExpectedResultTestPrimaryConstructorWithParameter();
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        [WorkItem(3785, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3785")]
        public async Task TestPrimaryConstructorWithoutParameterAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo(
    )
{{
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.ReferenceTypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        [WorkItem(3785, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3785")]
        public async Task TestPrimaryConstructorBaseListWithArgumentsAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo(int x)
{{
}}

{typeKeyword} Bar(int x) : Foo(x
    {{|#0:)|}}
{{
}}";

            var fixedCode = $@"
{typeKeyword} Foo(int x)
{{
}}

{typeKeyword} Bar(int x) : Foo(x)
{{
}}";

            var expected = this.GetExpectedResultTestPrimaryConstructorBaseList();
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.ReferenceTypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        [WorkItem(3785, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3785")]
        public async Task TestPrimaryConstructorBaseListWithoutArgumentsAsync(string typeKeyword)
        {
            var testCode = $@"
{typeKeyword} Foo()
{{
}}

{typeKeyword} Bar(int x) : Foo(
    )
{{
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestPrimaryConstructorWithParameter()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/53136
                Diagnostic().WithLocation(0),
                Diagnostic().WithLocation(0),
            };
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestPrimaryConstructorBaseList()
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
