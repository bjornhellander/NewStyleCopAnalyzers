// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1021NegativeSignsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1021CSharp14UnitTests
    {
        [Fact]
        public async Task TestFieldUnaryMinusAsync()
        {
            var testCode = @"
public class TestClass
{
    public int Prop
    {
        get => {|#0:-|} field;
    }
}
";

            var fixedCode = @"
public class TestClass
{
    public int Prop
    {
        get => -field;
    }
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments(" not", "followed");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
