// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1113CommaMustBeOnSameLineAsPreviousParameter,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1113CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that the commas of a positional pattern, which C# 8 introduced, are inspected the same way as
        /// the commas of a parameter list.
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
        return value is Point(1
            [|,|] 2);
    }
}
";

            var fixedCode = @"public class Point
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
            2);
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
