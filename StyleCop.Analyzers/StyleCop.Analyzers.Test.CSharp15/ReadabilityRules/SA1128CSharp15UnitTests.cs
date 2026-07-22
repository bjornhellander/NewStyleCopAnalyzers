// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1128ConstructorInitializerMustBeOnOwnLine,
        StyleCop.Analyzers.ReadabilityRules.SA1128CodeFixProvider>;

    public partial class SA1128CSharp15UnitTests : SA1128CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionConstructorInitializerNotOnOwnLineAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public TestUnion() {|#0:: this(string.Empty)|}
    {
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public TestUnion()
        : this(string.Empty)
    {
    }
}
";

            // TODO: Report bug - The compiler calls the registered constructor declaration action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
