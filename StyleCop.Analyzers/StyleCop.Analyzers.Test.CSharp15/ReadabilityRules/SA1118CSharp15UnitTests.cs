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
        StyleCop.Analyzers.ReadabilityRules.SA1118ParameterMustNotSpanMultipleLines>;

    public partial class SA1118CSharp15UnitTests : SA1118CSharp14UnitTests
    {
        [Fact]
        public async Task TestCollectionExpressionWithElementSecondArgumentSpansMultipleLinesAsync()
        {
            var testCode = @"
using System;
using System.Collections.Generic;

public class Foo
{
    public void Bar()
    {
        HashSet<string> set = [with(10,
            [|StringComparer
                .Ordinal|])];
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestCollectionExpressionWithElementSecondArgumentIsExemptInvocationExpressionAsync()
        {
            var testCode = @"
using System.Collections.Generic;

public class Foo
{
    public void Bar()
    {
        HashSet<string> set = [with(10,
            EqualityComparer<string>.Create(
                (a, b) => a == b))];
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
