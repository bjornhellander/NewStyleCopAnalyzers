// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.CodeAnalysis.Testing;

    using Xunit;

    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1119StatementMustNotUseUnnecessaryParenthesis,
        StyleCop.Analyzers.MaintainabilityRules.SA1119CodeFixProvider>;

    public partial class SA1119CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that a type cast followed by a switch expression is handled correctly.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestTypeCastFollowedBySwitchExpressionIsHandledCorrectlyAsync()
        {
            const string testCode = @"
public class Foo
{
    public string TestMethod(int n, object a, object b)
    {
        return (string)(n switch { 1 => a, 2 => b });
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a type cast followed by a switch expression with unnecessary parenthesis is handled correctly.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestTypeCastFollowedBySwitchExpressionWithUnnecessaryParenthesisIsHandledCorrectlyAsync()
        {
            const string testCode = @"
public class Foo
{
    public string TestMethod(int n, object a, object b)
    {
        return (string)((n switch { 1 => a, 2 => b }));
    }
}
";

            const string fixedCode = @"
public class Foo
{
    public string TestMethod(int n, object a, object b)
    {
        return (string)(n switch { 1 => a, 2 => b });
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic(DiagnosticId).WithSpan(6, 24, 6, 55),
                Diagnostic(ParenthesesDiagnosticId).WithLocation(6, 24),
                Diagnostic(ParenthesesDiagnosticId).WithLocation(6, 54),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that <see langword="await"/> followed by a switch expression is handled correctly.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestAwaitFollowedBySwitchExpressionIsHandledCorrectlyAsync()
        {
            const string testCode = @"
using System.Threading.Tasks;

public class Foo
{
    public async Task<string> TestMethod(int n, Task<string> a, Task<string> b)
    {
        return await (n switch { 1 => a, 2 => b });
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that <see langword="await"/> followed by a switch expression with unnecessary parenthesis is handled correctly.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestAwaitFollowedBySwitchExpressionWithUnnecessaryParenthesisIsHandledCorrectlyAsync()
        {
            const string testCode = @"
using System.Threading.Tasks;

public class Foo
{
    public async Task<string> TestMethod(int n, Task<string> a, Task<string> b)
    {
        return await {|#0:{|#1:(|}(n switch { 1 => a, 2 => b }){|#2:)|}|};
    }
}
";

            const string fixedCode = @"
using System.Threading.Tasks;

public class Foo
{
    public async Task<string> TestMethod(int n, Task<string> a, Task<string> b)
    {
        return await (n switch { 1 => a, 2 => b });
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic(DiagnosticId).WithLocation(0),
                Diagnostic(ParenthesesDiagnosticId).WithLocation(1),
                Diagnostic(ParenthesesDiagnosticId).WithLocation(2),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a switch expression with unnecessary parenthesis is handled correcly.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestSwitchExpressionWithUnnecessaryParenthesisAsync()
        {
            const string testCode = @"
public class Foo
{
    public void TestMethod(int n, object a, object b)
    {
        var test = (n switch { 1 => a, 2 => b });
    }
}
";

            const string fixedCode = @"
public class Foo
{
    public void TestMethod(int n, object a, object b)
    {
        var test = n switch { 1 => a, 2 => b };
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic(DiagnosticId).WithSpan(6, 20, 6, 49),
                Diagnostic(ParenthesesDiagnosticId).WithLocation(6, 20),
                Diagnostic(ParenthesesDiagnosticId).WithLocation(6, 48),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [InlineData(".ToString()")]
        [InlineData("?.ToString()")]
        [InlineData("[0]")]
        [InlineData("?[0]")]
        public async Task TestSwitchExpressionFollowedByDereferenceAsync(string operation)
        {
            string testCode = $@"
public class Foo
{{
    public object TestMethod(int n, string a, string b)
    {{
        return (n switch {{ 1 => a, 2 => b }}){operation};
    }}
}}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestSwitchExpressionFollowedByPointerDereferenceAsync()
        {
            string testCode = @"
public class Foo
{
    public unsafe string TestMethod(int n, byte* a, byte* b)
    {
        return (n switch { 1 => a, 2 => b })->ToString();
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestSwitchExpressionFollowedByInvocationAsync()
        {
            string testCode = @"
using System;

public class Foo
{
    public string TestMethod(int n, Func<string> a, Func<string> b)
    {
        return (n switch { 1 => a, 2 => b })();
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestStackAllocExpressionInExpressionAsync()
        {
            const string testCode = @"public class TestClass
{
    public unsafe void TestMethod()
    {
        var ptr1 = stackalloc byte[1];
        var span1 = (stackalloc byte[1]);
        var ptr2 = stackalloc[] { 0 };
        var span2 = (stackalloc[] { 0 });
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestRangeFollowedByMemberCallAsync()
        {
            const string testCode = @"using System;

public class TestClass
{
    public void TestMethod()
    {
        var array = (1..10).ToString();
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestRangeAsync()
        {
            const string testCode = @"using System;

public class TestClass
{
    public void TestMethod()
    {
        var a = new int[0];
        var b = a[(..)];
        var range = (1..10);
    }
}
";

            const string fixedCode = @"using System;

public class TestClass
{
    public void TestMethod()
    {
        var a = new int[0];
        var b = a[..];
        var range = 1..10;
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic(DiagnosticId).WithSpan(8, 19, 8, 23),
                Diagnostic(ParenthesesDiagnosticId).WithLocation(8, 19),
                Diagnostic(ParenthesesDiagnosticId).WithLocation(8, 22),
                Diagnostic(DiagnosticId).WithSpan(9, 21, 9, 28),
                Diagnostic(ParenthesesDiagnosticId).WithLocation(9, 21),
                Diagnostic(ParenthesesDiagnosticId).WithLocation(9, 27),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
