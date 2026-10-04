// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    using static StyleCop.Analyzers.SpacingRules.SA1008OpeningParenthesisMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1008OpeningParenthesisMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1008CSharp10UnitTests
    {
        [Fact]
        public async Task TestLambdaWithAttributeAsync()
        {
            var testCode = @"using System;

public class TestClass
{
    public void TestMethod()
    {
        var a = [Obsolete]{|#0:(|}) => 1;
        var b = [Obsolete] () => 1;
    }
}";

            var fixedCode = @"using System;

public class TestClass
{
    public void TestMethod()
    {
        var a = [Obsolete] () => 1;
        var b = [Obsolete] () => 1;
    }
}";

            var expected = Diagnostic(DescriptorPreceded).WithLocation(0);
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
