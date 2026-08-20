// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1206DeclarationKeywordsMustFollowOrder,
        StyleCop.Analyzers.OrderingRules.SA1206CodeFixProvider>;

    public partial class SA1206CSharp15UnitTests
    {
        [Fact]
        public async Task VerifyClosedKeywordReorderingInClassDeclarationAsync()
        {
            var testCode = @"
closed {|#0:public|} class GateState
{
}
";

            var fixedCode = @"
public closed class GateState
{
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("public", "closed");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
