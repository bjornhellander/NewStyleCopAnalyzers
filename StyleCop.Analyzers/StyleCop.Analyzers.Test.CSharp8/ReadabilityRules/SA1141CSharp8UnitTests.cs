// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1141UseTupleSyntax,
        StyleCop.Analyzers.ReadabilityRules.SA1141CodeFixProvider>;

    public partial class SA1141CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that a tuple pattern, which C# 8 introduced, is not reported. The analyzer inspects type
        /// syntax, not patterns, so the pattern itself is never a candidate for tuple syntax.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestTuplePatternAsync()
        {
            var testCode = @"public class TestClass
{
    public bool TestMethod((int, int) value)
    {
        return value is (1, 2);
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
