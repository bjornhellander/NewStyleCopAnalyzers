// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1001CommasMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1001CSharp14UnitTests
    {
        [Fact]
        public async Task TestSimpleLambdaParametersWithRefAndOutModifiersAsync()
        {
            var testCode = @"
public delegate void RefOutAction(int a, out int b);

public class TestClass
{
    public void Method()
    {
        RefOutAction action = (a{|#0:,|}out b) => { b = 2; };
    }
}
";

            var fixedCode = @"
public delegate void RefOutAction(int a, out int b);

public class TestClass
{
    public void Method()
    {
        RefOutAction action = (a, out b) => { b = 2; };
    }
}
";

            var expected = Diagnostic().WithArguments(string.Empty, "followed").WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
