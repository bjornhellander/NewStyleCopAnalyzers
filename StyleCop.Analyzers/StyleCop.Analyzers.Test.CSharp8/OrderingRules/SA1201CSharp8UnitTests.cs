// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1201ElementsMustAppearInTheCorrectOrder,
        StyleCop.Analyzers.OrderingRules.ElementOrderCodeFixProvider>;

    public partial class SA1201CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that readonly instance members, which C# 8 introduced, are ordered by element kind like any other member.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestReadonlyInstanceMemberAsync()
        {
            var testCode = @"public struct TestStruct
{
    public readonly int Method() => 0;

    public readonly int {|#0:Property|} => 0;
}
";

            var fixedCode = @"public struct TestStruct
{
    public readonly int Property => 0;

    public readonly int Method() => 0;
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("property", "method");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
