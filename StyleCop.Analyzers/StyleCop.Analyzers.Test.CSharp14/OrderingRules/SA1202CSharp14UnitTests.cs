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
        StyleCop.Analyzers.OrderingRules.SA1202ElementsMustBeOrderedByAccess,
        StyleCop.Analyzers.OrderingRules.ElementOrderCodeFixProvider>;

    public partial class SA1202CSharp14UnitTests : SA1202CSharp13UnitTests
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
        public async Task TestPropertyBeforeExtensionDeclarationAsync()
        {
            string testCode = @"
public static class TestClass
{
    public static int TestProperty => 0;

    extension(string source)
    {
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestFieldBeforeExtensionDeclarationAsync()
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

    private static int TestField;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestTwoExtensionDeclarationsAsync()
        {
            string testCode = @"
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

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
