// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1307AccessibleFieldsMustBeginWithUpperCaseLetter,
        StyleCop.Analyzers.NamingRules.RenameToUpperCaseCodeFixProvider>;

    public partial class SA1307CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionAccessibleFieldStartingWithLowerCaseLetterAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static int {|#0:myField|};
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static int MyField;
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("myField");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
