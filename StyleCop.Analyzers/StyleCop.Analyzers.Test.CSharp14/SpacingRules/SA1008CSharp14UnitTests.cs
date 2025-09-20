// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp13.SpacingRules;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.SA1008OpeningParenthesisMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1008OpeningParenthesisMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1008CSharp14UnitTests : SA1008CSharp13UnitTests
    {
        [Fact]
        public async Task TestExtensionDeclarationWithSpaceBeforeParenthesisAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension {|#0:(|}string source)
    {
    }
}
";

            var fixedCode = @"
public static class TestClass
{
    extension(string source)
    {
    }
}
";

            var expected = Diagnostic(DescriptorNotPreceded).WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestExtensionDeclarationWithSpaceAfterParenthesisAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension{|#0:(|} string source)
    {
    }
}
";

            var fixedCode = @"
public static class TestClass
{
    extension(string source)
    {
    }
}
";

            var expected = Diagnostic(DescriptorNotFollowed).WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
