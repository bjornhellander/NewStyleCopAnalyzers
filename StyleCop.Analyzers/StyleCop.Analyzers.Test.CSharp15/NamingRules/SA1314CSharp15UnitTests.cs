// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.NamingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1314TypeParameterNamesMustBeginWithT,
        StyleCop.Analyzers.NamingRules.SA1314CodeFixProvider>;

    public partial class SA1314CSharp15UnitTests : SA1314CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionMethodTypeParameterNotStartingWithTAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod<{|#0:X|}>()
    {
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod<TX>()
    {
    }
}
";

            // TODO: Report bug - The compiler calls the registered type parameter action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
