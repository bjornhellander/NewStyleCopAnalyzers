// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1204StaticElementsMustAppearBeforeInstanceElements,
        StyleCop.Analyzers.OrderingRules.ElementOrderCodeFixProvider>;

    public partial class SA1204CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that a readonly instance member, which C# 8 introduced, is ordered as an instance member.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestReadonlyInstanceMemberAsync()
        {
            var testCode = @"public struct TestStruct
{
    public readonly int Method() => 0;

    public static int [|StaticMethod|]() => 0;
}
";

            var fixedCode = @"public struct TestStruct
{
    public static int StaticMethod() => 0;

    public readonly int Method() => 0;
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
