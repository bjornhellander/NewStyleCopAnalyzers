// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp13.LayoutRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1500BracesForMultiLineStatementsMustNotShareLine,
        StyleCop.Analyzers.LayoutRules.SA1500CodeFixProvider>;

    public partial class SA1500CSharp14UnitTests : SA1500CSharp13UnitTests
    {
        [Fact]
        public async Task TestExtensionDeclarationAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension(string source) {
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

            // TODO: Syntax node actions seems to be triggered twice
            // Reported in https://github.com/dotnet/roslyn/issues/80319
            var expected = new[]
            {
                Diagnostic().WithSpan(4, 30, 4, 31),
                Diagnostic().WithSpan(4, 30, 4, 31),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
