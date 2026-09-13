// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1407ArithmeticExpressionsMustDeclarePrecedence,
        StyleCop.Analyzers.MaintainabilityRules.SA1407SA1408CodeFixProvider>;

    public partial class SA1407CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionMethodArithmeticExpressionWithoutPrecedenceAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        int x = 1 + [|1 * 1|];
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        int x = 1 + (1 * 1);
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
