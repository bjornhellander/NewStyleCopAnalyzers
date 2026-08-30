// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1111ClosingParenthesisMustBeOnLineOfLastParameter,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1111CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that a positional pattern, which C# 8 introduced, is not treated as a parameter list. The
        /// analyzer registers no pattern syntax kinds, so the closing parenthesis of a pattern is never inspected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        // TODO: Should this trigger?
        [Fact]
        public async Task TestMultiLinePositionalPatternAsync()
        {
            var testCode = @"public class Point
{
    public void Deconstruct(out int x, out int y)
    {
        x = 0;
        y = 0;
    }
}

public class TestClass
{
    public bool TestMethod(object value)
    {
        return value is Point(1,
            2
            );
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
