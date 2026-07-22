// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.NamingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1306FieldNamesMustBeginWithLowerCaseLetter,
        StyleCop.Analyzers.NamingRules.RenameToLowerCaseCodeFixProvider>;

    public partial class SA1306CSharp15UnitTests : SA1306CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionFieldStartingWithUpperCaseLetterAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static int {|#0:MyField|};
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    private static int myField;
}
";

            // TODO: Report bug - The compiler calls the registered field declaration action three times
            var expected = new[]
            {
                Diagnostic().WithLocation(0).WithArguments("MyField"),
                Diagnostic().WithLocation(0).WithArguments("MyField"),
                Diagnostic().WithLocation(0).WithArguments("MyField"),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
