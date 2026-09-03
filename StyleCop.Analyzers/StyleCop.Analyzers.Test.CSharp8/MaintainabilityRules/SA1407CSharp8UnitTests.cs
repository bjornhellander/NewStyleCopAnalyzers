// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1407ArithmeticExpressionsMustDeclarePrecedence,
        StyleCop.Analyzers.MaintainabilityRules.SA1407SA1408CodeFixProvider>;

    public partial class SA1407CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that arithmetic precedence is still checked on the right hand side of a null-coalescing
        /// assignment, which C# 8 introduced.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestNullCoalescingAssignmentAsync()
        {
            var testCode = @"public class Foo
{
    public void Bar(int? x)
    {
        x ??= 1 + [|1 * 1|];
    }
}";

            var fixedCode = @"public class Foo
{
    public void Bar(int? x)
    {
        x ??= 1 + (1 * 1);
    }
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
