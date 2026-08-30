// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1011ClosingSquareBracketsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1011CSharp8UnitTests
    {
        /// <summary>
        /// Verify that declaring a null reference type works for arrays.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task VerifyNullableContextWithArraysAsync()
        {
            var testCode = @"namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod()
        {
            byte[]? data = null;
        }
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task VerifyNullableContextWithArrayReturnsAsync()
        {
            var testCode = @"namespace TestNamespace
{
    public class TestClass
    {
        public byte[]? TestMethod()
        {
            return null;
        }
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestClosingSquareBracketFollowedByExclamationAsync()
        {
            var testCode = @"namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod(object?[] arguments)
        {
            object o1 = arguments[0] !;
            object o2 = arguments[0]! ;
            object o3 = arguments[0] ! ;
            string s1 = arguments[0] !.ToString();
            string s2 = arguments[0]! .ToString();
            string s3 = arguments[0] ! .ToString();
        }
    }
}
";

            var fixedCode = @"namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod(object?[] arguments)
        {
            object o1 = arguments[0]!;
            object o2 = arguments[0]! ;
            object o3 = arguments[0]! ;
            string s1 = arguments[0]!.ToString();
            string s2 = arguments[0]! .ToString();
            string s3 = arguments[0]! .ToString();
        }
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic().WithArguments(" not", "followed").WithLocation(7, 36),
                Diagnostic().WithArguments(" not", "followed").WithLocation(9, 36),
                Diagnostic().WithArguments(" not", "followed").WithLocation(10, 36),
                Diagnostic().WithArguments(" not", "followed").WithLocation(12, 36),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestClosingSquareBracketFollowedByRangeAsync()
        {
            var testCode = @"namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod(int[] arg)
        {
            _ = arg[0]..;
            _ = arg[0] ..;
        }
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies the handling of the closing bracket of a stackalloc in a nested expression, which C# 8 allows.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestStackAllocInNestedExpressionAsync()
        {
            var testCode = @"using System;

public class TestClass
{
    public void TestMethod()
    {
        Bar(stackalloc int[3 {|#0:]|});
    }

    public void Bar(Span<int> value)
    {
    }
}
";

            var fixedCode = @"using System;

public class TestClass
{
    public void TestMethod()
    {
        Bar(stackalloc int[3]);
    }

    public void Bar(Span<int> value)
    {
    }
}
";

            DiagnosticResult expected = Diagnostic().WithLocation(0).WithArguments(" not", "preceded");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
