// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1141UseTupleSyntax,
        StyleCop.Analyzers.ReadabilityRules.SA1141CodeFixProvider>;

    public partial class SA1141CSharp15UnitTests : SA1141CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionMethodValueTupleTypeAsync()
        {
            var testCode = @"
using System;

public union TestUnion(string, int)
{
    public static {|#0:ValueTuple<int, int>|} TestMethod()
    {
        return default;
    }
}
";

            var fixedCode = @"
using System;

public union TestUnion(string, int)
{
    public static (int, int) TestMethod()
    {
        return default;
    }
}
";

            // TODO: Report bug - The compiler calls the registered generic name action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
