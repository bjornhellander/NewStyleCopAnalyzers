// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1011ClosingSquareBracketsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1011CSharp11UnitTests
    {
        [Fact]
        public async Task TestListPatternInSwitchCaseAsync()
        {
            var testCode = @"public class TestClass
{
    public void TestMethod(object[] arg)
    {
        switch (arg)
        {
            case [string s{|#0:]|} :
                break;
        }
    }}
";

            var fixedCode = @"public class TestClass
{
    public void TestMethod(object[] arg)
    {
        switch (arg)
        {
            case [string s]:
                break;
        }
    }}
";

            var expected = Diagnostic().WithLocation(0).WithArguments(" not", "followed");
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
