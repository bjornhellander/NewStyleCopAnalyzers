// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.SA1010OpeningSquareBracketsMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1010OpeningSquareBracketsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1010CSharp8UnitTests
    {
        /// <summary>
        /// Verifies the handling of a stackalloc of a constructed unmanaged type, which C# 8 allows.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestStackAllocOfConstructedUnmanagedTypeAsync()
        {
            var testCode = @"public struct Foo<T>
{
    public T Value;
}

public class TestClass
{
    public unsafe void TestMethod()
    {
        Foo<int>* data1 = stackalloc Foo<int> {|#0:[|}3];
        Foo<int>* data2 = stackalloc Foo<int>{|#1:[|} 3];
    }
}
";

            var fixedCode = @"public struct Foo<T>
{
    public T Value;
}

public class TestClass
{
    public unsafe void TestMethod()
    {
        Foo<int>* data1 = stackalloc Foo<int>[3];
        Foo<int>* data2 = stackalloc Foo<int>[3];
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic(DescriptorNotPreceded).WithLocation(0),
                Diagnostic(DescriptorNotFollowed).WithLocation(1),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies the handling of an array of a constructed unmanaged type accessed through a pointer.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestPointerIndexingOfConstructedUnmanagedTypeAsync()
        {
            var testCode = @"public struct Foo<T>
{
    public T Value;
}

public class TestClass
{
    public unsafe void TestMethod(Foo<int>* data)
    {
        var value1 = data {|#0:[|}0].Value;
        var value2 = data{|#1:[|} 0].Value;
    }
}
";

            var fixedCode = @"public struct Foo<T>
{
    public T Value;
}

public class TestClass
{
    public unsafe void TestMethod(Foo<int>* data)
    {
        var value1 = data[0].Value;
        var value2 = data[0].Value;
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic(DescriptorNotPreceded).WithLocation(0),
                Diagnostic(DescriptorNotFollowed).WithLocation(1),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies the handling of the opening bracket of a stackalloc in a nested expression, which C# 8 allows.
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
        Bar(stackalloc int {|#0:[|}3]);
        Bar(stackalloc int{|#1:[|} 3]);
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
        Bar(stackalloc int[3]);
    }

    public void Bar(Span<int> value)
    {
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic(DescriptorNotPreceded).WithLocation(0),
                Diagnostic(DescriptorNotFollowed).WithLocation(1),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies the handling of an index-from-end argument, which C# 8 introduced.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestIndexFromEndAsync()
        {
            var testCode = @"public class TestClass
{
    public int TestMethod(int[] values)
    {
        var value1 = values {|#0:[|}^1];
        var value2 = values{|#1:[|} ^1];
        return value1 + value2 + values[^1];
    }
}
";

            var fixedCode = @"public class TestClass
{
    public int TestMethod(int[] values)
    {
        var value1 = values[^1];
        var value2 = values[^1];
        return value1 + value2 + values[^1];
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic(DescriptorNotPreceded).WithLocation(0),
                Diagnostic(DescriptorNotFollowed).WithLocation(1),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies the handling of a range argument, which C# 8 introduced.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestRangeAsync()
        {
            var testCode = @"public class TestClass
{
    public void TestMethod(int[] values)
    {
        var range1 = values {|#0:[|}1..2];
        var range2 = values{|#1:[|} ..^1];
        var range3 = TestMethod2(values {|#2:[|}..]);
    }

    public int TestMethod2(int[] values) => 0;
}
";

            var fixedCode = @"public class TestClass
{
    public void TestMethod(int[] values)
    {
        var range1 = values[1..2];
        var range2 = values[..^1];
        var range3 = TestMethod2(values[..]);
    }

    public int TestMethod2(int[] values) => 0;
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic(DescriptorNotPreceded).WithLocation(0),
                Diagnostic(DescriptorNotFollowed).WithLocation(1),
                Diagnostic(DescriptorNotPreceded).WithLocation(2),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
