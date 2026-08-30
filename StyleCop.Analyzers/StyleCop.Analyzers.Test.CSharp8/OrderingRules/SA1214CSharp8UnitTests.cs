// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1214ReadonlyElementsMustAppearBeforeNonReadonlyElements,
        StyleCop.Analyzers.OrderingRules.ElementOrderCodeFixProvider>;

    public partial class SA1214CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that the rule orders readonly fields only, and that a readonly instance member,
        /// which C# 8 introduced, is not treated as a readonly element.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestReadonlyInstanceMemberAsync()
        {
            var testCode = @"public struct TestStruct
{
    public int Field;

    public readonly int [|ReadonlyField|];

    public readonly int Method() => 0;
}
";

            var fixedCode = @"public struct TestStruct
{
    public readonly int ReadonlyField;

    public int Field;

    public readonly int Method() => 0;
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
