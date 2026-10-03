// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1124DoNotUseRegions,
        StyleCop.Analyzers.ReadabilityRules.RemoveRegionCodeFixProvider>;

    public partial class SA1124CSharp9UnitTests
    {
        /// <summary>
        /// Verifies that a region around top-level statements is not reported, since it is reported by SA1123 like a
        /// region in a method body.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestRegionAroundTopLevelStatementsAsync()
        {
            var testCode = @"System.Console.WriteLine(1);
#region Foo
System.Console.WriteLine(2);
#endregion
System.Console.WriteLine(3);
";

            await VerifyTopLevelProgramAsync(testCode, testCode).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestRegionAroundLastTopLevelStatementsAsync()
        {
            var testCode = @"System.Console.WriteLine(1);
#region Foo
System.Console.WriteLine(2);
#endregion
";

            await VerifyTopLevelProgramAsync(testCode, testCode).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a region within a top-level statement without a block is reported, just like a region in an
        /// expression-bodied method.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestRegionInExpressionBodiedLocalFunctionAsync()
        {
            var testCode = @"int F() =>
[|#region Foo|]
    1;
#endregion

System.Console.WriteLine(F());
";

            var fixedCode = @"int F() => 1;

System.Console.WriteLine(F());
";

            await VerifyTopLevelProgramAsync(testCode, fixedCode).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestRegionAroundTypeAfterTopLevelStatementsAsync()
        {
            var testCode = @"System.Console.WriteLine();

[|#region Foo|]
class TestType
{
}
#endregion

class OtherType
{
}
";

            var fixedCode = @"System.Console.WriteLine();

class TestType
{
}

class OtherType
{
}
";

            await VerifyTopLevelProgramAsync(testCode, fixedCode).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestRegionSpanningTopLevelStatementsAndTypeAsync()
        {
            var testCode = @"[|#region Foo|]
System.Console.WriteLine();

class TestType
{
}
#endregion

class OtherType
{
}
";

            var fixedCode = @"System.Console.WriteLine();

class TestType
{
}

class OtherType
{
}
";

            await VerifyTopLevelProgramAsync(testCode, fixedCode).ConfigureAwait(true);
        }

        private static Task VerifyTopLevelProgramAsync(string testCode, string fixedCode)
        {
            return new CSharpTest()
            {
                TestState =
                {
                    OutputKind = OutputKind.ConsoleApplication,
                    Sources = { testCode },
                },
                FixedCode = fixedCode,
            }.RunAsync(CancellationToken.None);
        }
    }
}
