// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.SpacingRules;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.SA1008OpeningParenthesisMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1008OpeningParenthesisMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1008CSharp15UnitTests : SA1008CSharp14UnitTests
    {
        [Fact]
        public async Task TestCollectionExpressionWithElementSpaceBeforeParenthesisAsync()
        {
            var testCode = @"
using System.Collections.Generic;

public class Foo
{
    public void Bar()
    {
        List<string> names = [with {|#0:(|}capacity: 10), ""a""];
    }
}";

            var fixedCode = @"
using System.Collections.Generic;

public class Foo
{
    public void Bar()
    {
        List<string> names = [with(capacity: 10), ""a""];
    }
}";

            var expected = Diagnostic(DescriptorNotPreceded).WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
