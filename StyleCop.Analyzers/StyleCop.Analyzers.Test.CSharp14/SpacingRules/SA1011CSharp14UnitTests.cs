// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1011ClosingSquareBracketsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1011CSharp14UnitTests
    {
        [Fact]
        public async Task TestFieldIndexerAsync()
        {
            var testCode = @"
public class TestClass
{
    private int result;

    public int[] Items
    {
        get => field;
        set
        {
            field = value;
            this.result = field[0 {|#0:]|};
        }
    }
}
";

            var fixedCode = @"
public class TestClass
{
    private int result;

    public int[] Items
    {
        get => field;
        set
        {
            field = value;
            this.result = field[0];
        }
    }
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments(" not", "preceded");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
