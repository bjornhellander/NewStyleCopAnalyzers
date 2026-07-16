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
        StyleCop.Analyzers.ReadabilityRules.SA1117ParametersMustBeOnSameLineOrSeparateLines>;

    public partial class SA1117CSharp15UnitTests : SA1117CSharp14UnitTests
    {
        [Fact]
        public async Task TestCollectionExpressionWithElementArgumentsNotAllOnSameLineOrSeparateLinesAsync()
        {
            var testCode = @"
using System;
using System.Collections.Concurrent;

public class Foo
{
    public void Bar()
    {
        ConcurrentDictionary<string, string> map = [with(1, 10,
            {|#0:StringComparer.Ordinal|})];
    }
}";

            var expected = Diagnostic().WithLocation(0);

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestCollectionExpressionWithElementArgumentsAllOnSeparateLinesAsync()
        {
            var testCode = @"
using System;
using System.Collections.Concurrent;

public class Foo
{
    public void Bar()
    {
        ConcurrentDictionary<string, string> map = [with(
            1,
            10,
            StringComparer.Ordinal)];
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
