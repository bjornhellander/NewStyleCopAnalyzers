// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.NamingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1308VariableNamesMustNotBePrefixed,
        StyleCop.Analyzers.NamingRules.SA1308CodeFixProvider>;

    public partial class SA1308CSharp15UnitTests : SA1308CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionFieldPrefixedAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static int {|#0:s_myField|};
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
                Diagnostic().WithLocation(0).WithArguments("s_myField", "s_"),
                Diagnostic().WithLocation(0).WithArguments("s_myField", "s_"),
                Diagnostic().WithLocation(0).WithArguments("s_myField", "s_"),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
