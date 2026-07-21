// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1113CommaMustBeOnSameLineAsPreviousParameter,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1113CSharp15UnitTests : SA1113CSharp14UnitTests
    {
        [Fact]
        public async Task TestCollectionExpressionWithElementCommaPlacedAtTheSameLineAsTheNextArgumentAsync()
        {
            var testCode = @"
using System;
using System.Collections.Generic;

public class Foo
{
    public void Bar()
    {
        HashSet<string> set = [with(10
            [|,|] StringComparer.Ordinal)];
    }
}";

            var fixedCode = @"
using System;
using System.Collections.Generic;

public class Foo
{
    public void Bar()
    {
        HashSet<string> set = [with(10,
            StringComparer.Ordinal)];
    }
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionMethodCommaNotOnSameLineAsPreviousParameterAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod(string s
                    {|#0:,|} int i)
    {
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod(string s,
                    int i)
    {
    }
}
";

            // TODO: Report bug - The compiler calls the registered token action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
