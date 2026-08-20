// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1000KeywordsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1000CSharp13UnitTests
    {
        [Fact]
        public async Task TestAllowsRefStructConstraintRefKeywordTrailingCommentAsync()
        {
            var testCode = @"
class Foo<T>
    where T : allows {|#0:ref|}/*comment*/struct
{
}";
            var fixedCode = @"
class Foo<T>
    where T : allows ref /*comment*/struct
{
}";

            var expected = Diagnostic().WithLocation(0).WithArguments("ref", string.Empty, "followed");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
