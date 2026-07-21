// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.NamingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1312VariableNamesMustBeginWithLowerCaseLetter,
        StyleCop.Analyzers.NamingRules.RenameToLowerCaseCodeFixProvider>;

    public partial class SA1312CSharp15UnitTests : SA1312CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionLocalVariableStartingWithUpperCaseLetterAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        int {|#0:MyVariable|} = 1;
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        int myVariable = 1;
    }
}
";

            // TODO: Report bug - The compiler calls the registered variable declaration action three times
            var expected = new[]
            {
                Diagnostic().WithLocation(0).WithArguments("MyVariable"),
                Diagnostic().WithLocation(0).WithArguments("MyVariable"),
                Diagnostic().WithLocation(0).WithArguments("MyVariable"),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
