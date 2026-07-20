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
    }
}
