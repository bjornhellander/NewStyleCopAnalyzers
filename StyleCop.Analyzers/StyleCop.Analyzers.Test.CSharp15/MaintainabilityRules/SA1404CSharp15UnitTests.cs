// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.MaintainabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1404CodeAnalysisSuppressionMustHaveJustification>;

    public partial class SA1404CSharp15UnitTests : SA1404CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionMethodWithSuppressionMissingJustificationAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    [{|#0:System.Diagnostics.CodeAnalysis.SuppressMessage(null, null)|}]
    public static void TestMethod()
    {
    }
}
";

            // TODO: Report bug - The compiler calls the registered attribute action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionMethodWithSuppressionHavingJustificationAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, Justification = ""a justification"")]
    public static void TestMethod()
    {
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
