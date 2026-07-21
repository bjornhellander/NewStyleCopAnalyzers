// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1132DoNotCombineFields,
        StyleCop.Analyzers.ReadabilityRules.SA1132CodeFixProvider>;

    public partial class SA1132CSharp15UnitTests : SA1132CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionCombinedFieldsAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    {|#0:private static int a, b;|}
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    private static int a;
    private static int b;
}
";

            // TODO: Report bug - The compiler calls the registered field declaration action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
