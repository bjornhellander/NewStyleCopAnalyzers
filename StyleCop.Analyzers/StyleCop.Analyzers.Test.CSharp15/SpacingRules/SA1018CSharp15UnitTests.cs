// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.SpacingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1018NullableTypeSymbolsMustNotBePrecededBySpace,
        StyleCop.Analyzers.SpacingRules.SA1018CodeFixProvider>;

    public partial class SA1018CSharp15UnitTests : SA1018CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionFieldNullableTypeSymbolPrecededBySpaceAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static int {|#0:?|} field;
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    private static int? field;
}
";

            // TODO: Report bug - The compiler calls the registered nullable type action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
