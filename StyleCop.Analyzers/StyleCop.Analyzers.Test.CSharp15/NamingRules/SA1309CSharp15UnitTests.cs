// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.NamingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1309FieldNamesMustNotBeginWithUnderscore,
        StyleCop.Analyzers.NamingRules.SA1309CodeFixProvider>;

    public partial class SA1309CSharp15UnitTests : SA1309CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionFieldStartingWithUnderscoreAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static int {|#0:_myField|};
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
                Diagnostic().WithLocation(0).WithArguments("_myField"),
                Diagnostic().WithLocation(0).WithArguments("_myField"),
                Diagnostic().WithLocation(0).WithArguments("_myField"),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
