// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1117ParametersMustBeOnSameLineOrSeparateLines>;

    public partial class SA1117CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that the subpatterns of a two-element positional pattern, which C# 8 introduced, are inspected
        /// the same way as the parameters of a parameter list. With only two elements, any relative placement of
        /// the first and second subpattern establishes a valid line pattern by definition, so no diagnostic is
        /// produced (matching the behavior for a two-parameter parameter list).
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
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

        /// <summary>
        /// Verifies that a diagnostic is produced when the subpatterns of a positional pattern, which C# 8
        /// introduced, are not all on the same line or each on a separate line.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestMultiLinePositionalPatternInvalidAsync()
        {
            var testCode = @"public class Point3
{
    public void Deconstruct(out int x, out int y, out int z)
    {
        x = 0;
        y = 0;
        z = 0;
    }
}

public class TestClass
{
    public bool TestMethod(object value)
    {
        return value is Point3(1, 2,
            [|3|]);
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
