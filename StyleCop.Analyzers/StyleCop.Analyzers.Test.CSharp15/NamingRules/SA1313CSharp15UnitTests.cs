// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.NamingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1313ParameterNamesMustBeginWithLowerCaseLetter,
        StyleCop.Analyzers.NamingRules.RenameToLowerCaseCodeFixProvider>;

    public partial class SA1313CSharp15UnitTests : SA1313CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionMethodParameterStartingWithUpperCaseLetterAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod(int {|#0:MyParameter|})
    {
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod(int myParameter)
    {
    }
}
";

            // TODO: Report bug - The compiler calls the registered parameter action three times
            var expected = new[]
            {
                Diagnostic().WithLocation(0).WithArguments("MyParameter"),
                Diagnostic().WithLocation(0).WithArguments("MyParameter"),
                Diagnostic().WithLocation(0).WithArguments("MyParameter"),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
