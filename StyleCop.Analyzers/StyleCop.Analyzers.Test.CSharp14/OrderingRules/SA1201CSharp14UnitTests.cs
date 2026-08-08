// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1201ElementsMustAppearInTheCorrectOrder,
        StyleCop.Analyzers.OrderingRules.ElementOrderCodeFixProvider>;

    public partial class SA1201CSharp14UnitTests
    {
        [Fact]
        public async Task TestPropertyAfterExtensionBlockDeclarationAsync()
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

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestExtensionBlockDeclarationAfterPropertyAsync()
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

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestExtensionBlockDeclarationAfterFieldAsync()
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

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestFieldAfterExtensionBlockDeclarationAsync()
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

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInstanceCompoundAssignmentOperatorAfterStaticOperatorAsync()
        {
            string testCode = @"
public class TestClass
{
    public int Value;

    public static TestClass operator +(TestClass a, int b)
    {
        return a;
    }

    public void operator +=(int x)
    {
        this.Value += x;
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInstanceCompoundAssignmentOperatorAfterMethodAsync()
        {
            string testCode = @"
public class TestClass
{
    public int Value;

    public void TestMethod()
    {
    }

    {|#0:public void operator +=(int x)
    {
        this.Value += x;
    }|}
}
";

            string fixedCode = @"
public class TestClass
{
    public int Value;

    public void operator +=(int x)
    {
        this.Value += x;
    }

    public void TestMethod()
    {
    }
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("operator", "method");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
