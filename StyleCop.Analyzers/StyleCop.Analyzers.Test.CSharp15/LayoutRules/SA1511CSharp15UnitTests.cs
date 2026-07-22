// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.LayoutRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1511WhileDoFooterMustNotBePrecededByBlankLine,
        StyleCop.Analyzers.LayoutRules.SA1511CodeFixProvider>;

    public partial class SA1511CSharp15UnitTests : SA1511CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionMethodWhileFooterPrecededByBlankLineAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        do
        {
        }

        {|#0:while|} (true);
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        do
        {
        }
        while (true);
    }
}
";

            // TODO: Report bug - The compiler calls the registered do statement action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
