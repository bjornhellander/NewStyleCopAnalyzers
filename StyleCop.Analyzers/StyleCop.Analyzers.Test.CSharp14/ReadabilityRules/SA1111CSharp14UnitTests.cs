// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp13.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1111ClosingParenthesisMustBeOnLineOfLastParameter,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1111CSharp14UnitTests : SA1111CSharp13UnitTests
    {
        [Fact]
        public async Task TestExtensionBlockDeclarationWithParameterNameAsync()
        {
            string testCode = @"
public static class TestClass
{
    extension(string source
        [|)|]
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

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestExtensionBlockDeclarationWithoutParameterNameAsync()
        {
            string testCode = @"
public static class TestClass
{
    extension(string
        [|)|]
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

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
