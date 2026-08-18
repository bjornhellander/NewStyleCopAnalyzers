// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1116SplitParametersMustStartOnLineAfterDeclaration,
        StyleCop.Analyzers.ReadabilityRules.SA1116CodeFixProvider>;

    public partial class SA1116CSharp13UnitTests
    {
        [Fact]
        public async Task TestSplitParamsCollectionParameterNotStartingOnNextLineAsync()
        {
            var testCode = @"
using System;

class Foo
{
    public Foo([|int a|],
        params ReadOnlySpan<int> s) { }
}";

            var fixedCode = @"
using System;

class Foo
{
    public Foo(
        int a,
        params ReadOnlySpan<int> s) { }
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
