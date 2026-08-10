// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.SA1010OpeningSquareBracketsMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1010OpeningSquareBracketsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1010CSharp13UnitTests
    {
        [Fact]
        public async Task TestImplicitIndexInitializerAsync()
        {
            var testCode = @"
public class TimerRemaining
{
    public int[] Buffer { get; set; } = new int[10];
}

public class TestClass
{
    public void TestMethod()
    {
        var countdown = new TimerRemaining()
        {
            Buffer =
            {
                {|#0:[|} ^1] = 0,
            },
        };
    }
}
";

            var fixedTestCode = @"
public class TimerRemaining
{
    public int[] Buffer { get; set; } = new int[10];
}

public class TestClass
{
    public void TestMethod()
    {
        var countdown = new TimerRemaining()
        {
            Buffer =
            {
                [^1] = 0,
            },
        };
    }
}
";

            var expected = Diagnostic(DescriptorNotFollowed).WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
