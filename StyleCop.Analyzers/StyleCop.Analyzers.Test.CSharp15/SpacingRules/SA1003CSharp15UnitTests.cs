// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1003SymbolsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.SA1003CodeFixProvider>;

    public partial class SA1003CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionMethodBinaryOperatorNotSpacedAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        bool b = true {|#0:&&|}false;
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        bool b = true && false;
    }
}
";

            var expected = Diagnostic().WithMessageFormat("Operator '{0}' should be followed by whitespace.").WithLocation(0).WithArguments("&&");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
