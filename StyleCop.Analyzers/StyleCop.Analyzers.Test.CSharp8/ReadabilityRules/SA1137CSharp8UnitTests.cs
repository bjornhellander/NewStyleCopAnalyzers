// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1137ElementsShouldHaveTheSameIndentation,
        StyleCop.Analyzers.ReadabilityRules.IndentationCodeFixProvider>;

    public partial class SA1137CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that the arms of a switch expression, which C# 8 introduced, are not required to share indentation. The analyzer registers SwitchStatement but not SwitchExpression.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestSwitchExpressionAsync()
        {
            var testCode = @"public class TestClass
{
    public int TestMethod(int value)
    {
        return value switch
        {
            0 => 0,
[|                |]_ => 1,
        };
    }
}
";

            var fixedCode = @"public class TestClass
{
    public int TestMethod(int value)
    {
        return value switch
        {
            0 => 0,
            _ => 1,
        };
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
