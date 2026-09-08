// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1404CodeAnalysisSuppressionMustHaveJustification>;

    public partial class SA1404CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionMethodWithSuppressionMissingJustificationAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    [[|System.Diagnostics.CodeAnalysis.SuppressMessage(null, null)|]]
    public static void TestMethod()
    {
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
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
