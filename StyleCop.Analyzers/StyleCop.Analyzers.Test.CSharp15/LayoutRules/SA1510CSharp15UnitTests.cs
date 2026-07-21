// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.LayoutRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1510ChainedStatementBlocksMustNotBePrecededByBlankLine,
        StyleCop.Analyzers.LayoutRules.SA1510CodeFixProvider>;

    public partial class SA1510CSharp15UnitTests : SA1510CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionMethodElsePrecededByBlankLineAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        if (true)
        {
        }

        {|#0:else|}
        {
        }
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        if (true)
        {
        }
        else
        {
        }
    }
}
";

            // TODO: Report bug - The compiler calls the registered else clause action three times
            var expected = new[]
            {
                Diagnostic().WithLocation(0).WithArguments("else"),
                Diagnostic().WithLocation(0).WithArguments("else"),
                Diagnostic().WithLocation(0).WithArguments("else"),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
