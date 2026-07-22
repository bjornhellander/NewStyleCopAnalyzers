// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SX1101DoNotPrefixLocalMembersWithThis,
        StyleCop.Analyzers.ReadabilityRules.SX1101CodeFixProvider>;

    public partial class SX1101CSharp15UnitTests : SX1101CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionUnnecessaryThisPrefixAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private int MyProperty => 42;

    public int TestMethod()
    {
        return {|#0:this|}.MyProperty;
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    private int MyProperty => 42;

    public int TestMethod()
    {
        return MyProperty;
    }
}
";

            // TODO: Report bug - The compiler calls the registered this expression action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
