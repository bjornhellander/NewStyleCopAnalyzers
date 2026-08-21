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

        [Fact]
        public async Task VerifySafeKeywordReorderingInExplicitLayoutFieldDeclarationAsync()
        {
            var testCode = @"
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
internal struct TestStruct
{
    [FieldOffset(0)]
    safe {|#0:public|} int Value;
}
";

            var fixedCode = @"
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
internal struct TestStruct
{
    [FieldOffset(0)]
    public safe int Value;
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("public", "safe");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
