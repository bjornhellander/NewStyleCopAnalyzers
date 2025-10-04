// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp13.SpacingRules;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.SA1015ClosingGenericBracketsMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1015ClosingGenericBracketsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1015CSharp14UnitTests : SA1015CSharp13UnitTests
    {
        [Fact]
        public async Task TestExtensionBlockDeclarationWithSpaceBeforeBracketAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension<T {|#0:>|}(T source)
    {
    }
}
";

            var fixedCode = @"
public static class TestClass
{
    extension<T>(T source)
    {
    }
}
";

            var expected = Diagnostic(DescriptorNotPreceded).WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestExtensionBlockDeclarationWithSpaceAfterBracketAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension<T{|#0:>|} (T source)
    {
    }
}
";

            var fixedCode = @"
public static class TestClass
{
    extension<T>(T source)
    {
    }
}
";

            var expected = Diagnostic(DescriptorNotFollowed).WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
