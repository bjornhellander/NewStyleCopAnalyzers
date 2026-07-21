// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1134AttributesMustNotShareLine,
        StyleCop.Analyzers.ReadabilityRules.SA1134CodeFixProvider>;

    public partial class SA1134CSharp15UnitTests : SA1134CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionMethodAttributesShareLineAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    [System.Obsolete] {|#0:[|}System.CLSCompliant(true)]
    public static void TestMethod()
    {
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    [System.Obsolete]
    [System.CLSCompliant(true)]
    public static void TestMethod()
    {
    }
}
";

            // TODO: Report bug - The compiler calls the registered attribute list action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
