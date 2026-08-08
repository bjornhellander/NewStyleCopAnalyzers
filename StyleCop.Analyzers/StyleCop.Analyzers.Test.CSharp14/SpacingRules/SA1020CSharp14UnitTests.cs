// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1020IncrementDecrementSymbolsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1020CSharp14UnitTests
    {
        [Fact]
        public async Task TestFieldPostfixIncrementAsync()
        {
            var testCode = @"
public class TestClass
{
    public int Prop
    {
        get => field;
        set
        {
            field = value;
            field {|#0:++|};
        }
    }
}
";

            var fixedCode = @"
public class TestClass
{
    public int Prop
    {
        get => field;
        set
        {
            field = value;
            field++;
        }
    }
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("Increment", "++", "preceded");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestFieldPrefixIncrementAsync()
        {
            var testCode = @"
public class TestClass
{
    public int Prop
    {
        get => field;
        set
        {
            field = value;
            {|#0:++|} field;
        }
    }
}
";

            var fixedCode = @"
public class TestClass
{
    public int Prop
    {
        get => field;
        set
        {
            field = value;
            ++field;
        }
    }
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("Increment", "++", "followed");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
