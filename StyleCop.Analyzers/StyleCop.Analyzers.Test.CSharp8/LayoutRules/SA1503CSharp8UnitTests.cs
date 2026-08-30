// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1503BracesMustNotBeOmitted,
        StyleCop.Analyzers.LayoutRules.SA1503CodeFixProvider>;

    public partial class SA1503CSharp8UnitTests
    {
        [Fact]
        public async Task TestNoDiagnosticForUsingDeclarationStatementAsync()
        {
            var testCode = @"
using System.IO;
public class Foo
{
    public void Method()
    {
        using var v = new MemoryStream();
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that the body of an await foreach statement, which C# 8 introduced, must be enclosed in braces.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestAwaitForEachStatementAsync()
        {
            var testCode = @"using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Test
{
    public async Task MethodAsync(IAsyncEnumerable<int> values)
    {
        await foreach (var value in values)
            [|Console.WriteLine(value);|]
    }
}";

            var fixedCode = @"using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Test
{
    public async Task MethodAsync(IAsyncEnumerable<int> values)
    {
        await foreach (var value in values)
        {
            Console.WriteLine(value);
        }
    }
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
