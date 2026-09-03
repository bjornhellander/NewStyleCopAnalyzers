// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1000KeywordsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1000CSharp8UnitTests
    {
        /// <summary>
        /// Verifies the handling of the stackalloc keyword before a constructed unmanaged type, which C# 8 allows.
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
        Foo<int>* data1 = {|#0:stackalloc|}@Foo<int>[3];
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
        Foo<int>* data1 = stackalloc @Foo<int>[3];
    }
}
";

            DiagnosticResult expected = Diagnostic().WithLocation(0).WithArguments("stackalloc", string.Empty, "followed");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies the handling of the sizeof keyword applied to a constructed unmanaged type, which C# 8 allows.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestSizeOfConstructedUnmanagedTypeAsync()
        {
            var testCode = @"public struct Foo<T>
{
    public T Value;
}

public class TestClass
{
    public unsafe void TestMethod()
    {
        var size1 = {|#0:sizeof|} (Foo<int>);
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
        var size1 = sizeof(Foo<int>);
    }
}
";

            DiagnosticResult expected = Diagnostic().WithLocation(0).WithArguments("sizeof", " not", "followed");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies the handling of the stackalloc keyword in a nested expression, which C# 8 allows.
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
        Bar({|#0:stackalloc|}@Int32[3]);
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
        Bar(stackalloc @Int32[3]);
    }

    public void Bar(Span<int> value)
    {
    }
}
";

            DiagnosticResult expected = Diagnostic().WithLocation(0).WithArguments("stackalloc", string.Empty, "followed");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies the handling of the using keyword of a using declaration, which C# 8 introduced.
        /// The keyword is followed by a type rather than by an opening parenthesis here.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestUsingDeclarationAsync()
        {
            var testCode = @"using System;

public class TestClass
{
    public void TestMethod()
    {
        {|#0:using|}@IDisposable resource = null;
    }
}
";

            var fixedCode = @"using System;

public class TestClass
{
    public void TestMethod()
    {
        using @IDisposable resource = null;
    }
}
";

            DiagnosticResult expected = Diagnostic().WithLocation(0).WithArguments("using", string.Empty, "followed");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies the handling of the using keyword of an await using declaration, which C# 8 introduced.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestAwaitUsingDeclarationAsync()
        {
            var testCode = @"using System;
using System.Threading.Tasks;

public class TestClass
{
    public async Task TestMethodAsync()
    {
        await {|#0:using|}@IAsyncDisposable resource = null;
    }
}
";

            var fixedCode = @"using System;
using System.Threading.Tasks;

public class TestClass
{
    public async Task TestMethodAsync()
    {
        await using @IAsyncDisposable resource = null;
    }
}
";

            DiagnosticResult expected = Diagnostic().WithLocation(0).WithArguments("using", string.Empty, "followed");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies the handling of the foreach keyword of an await foreach statement, which C# 8 introduced. The
        /// await and foreach keywords are adjacent here.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestAwaitForEachStatementAsync()
        {
            var testCode = @"using System.Collections.Generic;
using System.Threading.Tasks;

public class TestClass
{
    public async Task TestMethodAsync(IAsyncEnumerable<int> values)
    {
        await {|#0:foreach|}(var value in values)
        {
        }
    }
}
";

            var fixedCode = @"using System.Collections.Generic;
using System.Threading.Tasks;

public class TestClass
{
    public async Task TestMethodAsync(IAsyncEnumerable<int> values)
    {
        await foreach (var value in values)
        {
        }
    }
}
";

            DiagnosticResult expected = Diagnostic().WithLocation(0).WithArguments("foreach", string.Empty, "followed");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
