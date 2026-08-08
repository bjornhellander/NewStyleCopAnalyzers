// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1312VariableNamesMustBeginWithLowerCaseLetter,
        StyleCop.Analyzers.NamingRules.RenameToLowerCaseCodeFixProvider>;

    public partial class SA1312CSharp11UnitTests
    {
        [Fact]
        public async Task TestScopedRefLocalStartingWithUpperCaseLetterAsync()
        {
            var testCode = @"
public class TestClass
{
    public void Bar()
    {
        int value = 5;
        scoped ref int {|#0:Bar|} = ref value;
    }
}
";

            var fixedCode = @"
public class TestClass
{
    public void Bar()
    {
        int value = 5;
        scoped ref int bar = ref value;
    }
}
";

            var expected = Diagnostic().WithArguments("Bar").WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
