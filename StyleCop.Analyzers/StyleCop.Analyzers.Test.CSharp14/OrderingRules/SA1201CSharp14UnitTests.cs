// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp13.OrderingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1201ElementsMustAppearInTheCorrectOrder,
        StyleCop.Analyzers.OrderingRules.ElementOrderCodeFixProvider>;

    public partial class SA1201CSharp14UnitTests : SA1201CSharp13UnitTests
    {
        [Fact]
        public async Task TestPropertyAfterExtensionDeclarationAsync()
        {
            string testCode = @"
public static class TestClass
{
    extension(string source)
    {
    }

    public static int TestProperty => 0;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestExtensionDeclarationAfterPropertyAsync()
        {
            string testCode = @"
public static class TestClass
{
    public static int TestProperty => 0;

    {|#0:extension|}(string source)
    {
    }
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("extension", "property");

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestExtensionDeclarationAfterFieldAsync()
        {
            string testCode = @"
public static class TestClass
{
    private static int TestField;

    extension(string source)
    {
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestFieldAfterExtensionDeclarationAsync()
        {
            string testCode = @"
public static class TestClass
{
    extension(string source)
    {
    }

    private static int {|#0:TestField|};
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("field", "extension");

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
