// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1115ParameterMustFollowComma>;

    public partial class SA1115CSharp15UnitTests : SA1115CSharp14UnitTests
    {
        [Fact]
        public async Task TestCollectionExpressionWithElementBlankLineAfterCommaAsync()
        {
            var testCode = @"
using System;
using System.Collections.Generic;

public class Foo
{
    public void Bar()
    {
        HashSet<string> set = [with(10,

            [|StringComparer.Ordinal|])];
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestCollectionExpressionWithElementNoBlankLineAfterCommaAsync()
        {
            var testCode = @"
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

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionMethodBlankLineAfterCommaAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        Fun(10,

            {|#0:20|});
    }

    private static void Fun(int a, int b)
    {
    }
}
";

            // TODO: Report bug - The compiler calls the registered argument list action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionMethodNoBlankLineAfterCommaAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        Fun(10,
            20);
    }

    private static void Fun(int a, int b)
    {
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
