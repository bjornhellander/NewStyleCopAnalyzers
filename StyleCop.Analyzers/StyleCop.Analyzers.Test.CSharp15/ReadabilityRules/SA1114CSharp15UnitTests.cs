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
        StyleCop.Analyzers.ReadabilityRules.SA1114ParameterListMustFollowDeclaration>;

    public partial class SA1114CSharp15UnitTests : SA1114CSharp14UnitTests
    {
        [Fact]
        public async Task TestCollectionExpressionWithElementBlankLineBeforeFirstArgumentAsync()
        {
            var testCode = @"
using System.Collections.Generic;

public class Foo
{
    public void Bar()
    {
        List<string> names = [with(

            {|#0:capacity: 10|}), ""a""];
    }
}";

            var expected = Diagnostic().WithLocation(0);

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestCollectionExpressionWithElementFirstArgumentOnNextLineAsync()
        {
            var testCode = @"
using System.Collections.Generic;

public class Foo
{
    public void Bar()
    {
        List<string> names = [with(
            capacity: 10), ""a""];
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
