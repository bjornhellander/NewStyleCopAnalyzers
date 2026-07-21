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
        StyleCop.Analyzers.ReadabilityRules.SA1116SplitParametersMustStartOnLineAfterDeclaration,
        StyleCop.Analyzers.ReadabilityRules.SA1116CodeFixProvider>;

    public partial class SA1116CSharp15UnitTests : SA1116CSharp14UnitTests
    {
        [Fact]
        public async Task TestCollectionExpressionWithElementSplitArgumentsNotStartingOnNextLineAsync()
        {
            var testCode = @"
using System;
using System.Collections.Generic;

public class Foo
{
    public void Bar()
    {
        HashSet<string> set = [with([|10|],
            StringComparer.Ordinal)];
    }
}";

            var fixedCode = @"
using System;
using System.Collections.Generic;

public class Foo
{
    public void Bar()
    {
        HashSet<string> set = [with(
            10,
            StringComparer.Ordinal)];
    }
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionMethodSplitArgumentsNotStartingOnNextLineAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        Fun({|#0:10|},
            20);
    }

    private static void Fun(int a, int b)
    {
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        Fun(
            10,
            20);
    }

    private static void Fun(int a, int b)
    {
    }
}
";

            // TODO: Report bug - The compiler calls the registered argument list action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
