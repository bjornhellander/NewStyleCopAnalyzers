// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1100DoNotPrefixCallsWithBaseUnlessLocalImplementationExists,
        StyleCop.Analyzers.ReadabilityRules.SA1100CodeFixProvider>;

    public partial class SA1100CSharp15UnitTests : SA1100CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionBaseCallWithoutLocalOverrideAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public string TestMethod()
    {
        return {|#0:base|}.ToString();
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public string TestMethod()
    {
        return this.ToString();
    }
}
";

            // TODO: Report bug - The compiler calls the registered base expression action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
