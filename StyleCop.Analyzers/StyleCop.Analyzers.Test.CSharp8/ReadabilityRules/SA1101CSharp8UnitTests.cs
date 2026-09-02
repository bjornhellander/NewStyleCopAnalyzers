// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1101PrefixLocalCallsWithThis,
        StyleCop.Analyzers.ReadabilityRules.SA1101CodeFixProvider>;

    public partial class SA1101CSharp8UnitTests
    {
        [Fact]
        public async Task TestPropertyPatternAsync()
        {
            var testCode = @"public class Test
{
    public Test Inner;
    public string Value;

    public bool Method(Test arg)
    {
        return arg is { Value: """" };
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a local call in the body of an await foreach statement, which C# 8 introduced, must be
        /// prefixed with this.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestAwaitForEachStatementAsync()
        {
            var testCode = @"using System.Collections.Generic;
using System.Threading.Tasks;

public class Test
{
    public async Task MethodAsync(IAsyncEnumerable<int> values)
    {
        await foreach (var value in values)
        {
            [|Handle|](value);
        }
    }

    public void Handle(int value)
    {
    }
}";

            var fixedCode = @"using System.Collections.Generic;
using System.Threading.Tasks;

public class Test
{
    public async Task MethodAsync(IAsyncEnumerable<int> values)
    {
        await foreach (var value in values)
        {
            this.Handle(value);
        }
    }

    public void Handle(int value)
    {
    }
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a local call in the default implementation of an interface member, which C# 8 introduced,
        /// must be prefixed with this.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestDefaultInterfaceMethodAsync()
        {
            var testCode = @"public interface ITest
{
    void Method();

    void DefaultMethod()
    {
        [|Method|]();
    }
}";

            var fixedCode = @"public interface ITest
{
    void Method();

    void DefaultMethod()
    {
        this.Method();
    }
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
