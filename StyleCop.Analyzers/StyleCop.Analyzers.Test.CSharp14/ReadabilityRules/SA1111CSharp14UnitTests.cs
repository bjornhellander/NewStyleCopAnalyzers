// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp13.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1111ClosingParenthesisMustBeOnLineOfLastParameter,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1111CSharp14UnitTests : SA1111CSharp13UnitTests
    {
        [Fact]
        public async Task TestExtensionDeclarationWithParameterNameAsync()
        {
            string testCode = @"
public static class TestClass
{
    extension(string source
        {|#0:)|}
    {
    }
}
";

            string fixedCode = @"
public static class TestClass
{
    extension(string source)
    {
    }
}
";

            // TODO: Syntax node actions seems to be triggered twice
            // Reported in https://github.com/dotnet/roslyn/issues/80319
            var expected = new[]
            {
                Diagnostic().WithLocation(0),
                Diagnostic().WithLocation(0),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestExtensionDeclarationWithoutParameterNameAsync()
        {
            string testCode = @"
public static class TestClass
{
    extension(string
        {|#0:)|}
    {
    }
}
";

            string fixedCode = @"
public static class TestClass
{
    extension(string)
    {
    }
}
";

            // TODO: Syntax node actions seems to be triggered twice
            // Reported in https://github.com/dotnet/roslyn/issues/80319
            var expected = new[]
            {
                Diagnostic().WithLocation(0),
                Diagnostic().WithLocation(0),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
