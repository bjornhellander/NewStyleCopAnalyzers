// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.SpacingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1001CommasMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1001CSharp15UnitTests : SA1001CSharp14UnitTests
    {
        [Fact]
        public async Task TestCollectionExpressionWithElementSpaceBeforeCommaAsync()
        {
            var testCode = @"
using System;
using System.Collections.Generic;

public class Foo
{
    public void Bar()
    {
        HashSet<string> set = [with(10 {|#0:,|} StringComparer.Ordinal)];
    }
}";

            var fixedCode = @"
using System;
using System.Collections.Generic;

public class Foo
{
    public void Bar()
    {
        HashSet<string> set = [with(10, StringComparer.Ordinal)];
    }
}";

            var expected = Diagnostic().WithLocation(0).WithArguments(" not", "preceded");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
