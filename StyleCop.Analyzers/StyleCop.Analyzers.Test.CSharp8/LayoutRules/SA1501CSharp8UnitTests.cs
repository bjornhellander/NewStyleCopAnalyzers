// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1501StatementMustNotBeOnASingleLine,
        StyleCop.Analyzers.LayoutRules.SA1501CodeFixProvider>;

    public partial class SA1501CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that a single-line switch expression, which C# 8 introduced, is not inspected. The analyzer registers statement kinds, and a switch expression is an expression.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestSwitchExpressionAsync()
        {
            var testCode = @"public class TestClass
{
    public int TestMethod(int value)
    {
        return value switch { 0 => 0, _ => 1 };
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
