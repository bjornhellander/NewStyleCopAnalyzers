// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.SpacingRules;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.SA1010OpeningSquareBracketsMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1010OpeningSquareBracketsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1010CSharp15UnitTests : SA1010CSharp14UnitTests
    {
        [Fact]
        public async Task TestCollectionExpressionStartingWithWithElementAsync()
        {
            var testCode = @"
using System.Collections.Generic;

public class Foo
{
    public void Bar()
    {
        List<string> names = {|#0:[|} with(capacity: 10), ""a"", ""b""];
    }
}";

            var fixedCode = @"
using System.Collections.Generic;

public class Foo
{
    public void Bar()
    {
        List<string> names = [with(capacity: 10), ""a"", ""b""];
    }
}";

            var expected = Diagnostic(DescriptorNotFollowed).WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
