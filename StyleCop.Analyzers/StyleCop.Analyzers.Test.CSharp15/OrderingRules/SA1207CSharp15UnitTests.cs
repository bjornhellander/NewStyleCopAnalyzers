// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.OrderingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1207ProtectedMustComeBeforeInternal,
        StyleCop.Analyzers.OrderingRules.SA1207CodeFixProvider>;

    public partial class SA1207CSharp15UnitTests : SA1207CSharp14UnitTests
    {
        [Fact]
        public async Task TestNestedUnionWithInternalBeforeProtectedAsync()
        {
            var testCode = @"
public class Container
{
    internal {|#0:protected|} union TestUnion(string, int)
    {
    }
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("protected", "internal");

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestNestedUnionWithProtectedBeforeInternalAsync()
        {
            var testCode = @"
public class Container
{
    protected internal union TestUnion(string, int)
    {
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
