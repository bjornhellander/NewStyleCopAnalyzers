// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp13.LayoutRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1516ElementsMustBeSeparatedByBlankLine,
        StyleCop.Analyzers.LayoutRules.SA1516CodeFixProvider>;

    public partial class SA1516CSharp14UnitTests : SA1516CSharp13UnitTests
    {
        [Fact]
        public async Task TestExtensionBlockDeclarationsAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension(string source)
    {
    }
[|    |]extension(string source)
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

    extension(string source)
    {
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestInsideExtensionBlockDeclarationAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension(string source)
    {
        public int TestProperty1 => 0;
        public int TestProperty2 => 0;
    }
}
";

            var fixedCode = @"
public static class TestClass
{
    extension(string source)
    {
        public int TestProperty1 => 0;

        public int TestProperty2 => 0;
    }
}
";

            var expected = Diagnostic().WithSpan(7, 1, 7, 9);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
