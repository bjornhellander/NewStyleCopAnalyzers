// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.LayoutRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.LayoutRules.SA1519BracesMustNotBeOmittedFromMultiLineChildStatement>;

    public partial class SA1519CSharp15UnitTests : SA1519CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionMethodMultiLineChildStatementWithoutBracesAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        if (true)
            {|#0:System.Diagnostics.Debug.Assert(
                true);|}
    }
}
";

            // TODO: Report bug - The compiler calls the registered if statement action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionMethodMultiLineChildStatementWithBracesAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        if (true)
        {
            System.Diagnostics.Debug.Assert(
                true);
        }
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
