// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1121UseBuiltInTypeAlias,
        StyleCop.Analyzers.ReadabilityRules.SA1121CodeFixProvider>;

    public partial class SA1121CSharp15UnitTests : SA1121CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionFieldWithNonAliasTypeAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static {|#0:System.Int32|} field;
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    private static int field;
}
";

            // TODO: Report bug - The compiler calls the registered predefined/identifier name action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
