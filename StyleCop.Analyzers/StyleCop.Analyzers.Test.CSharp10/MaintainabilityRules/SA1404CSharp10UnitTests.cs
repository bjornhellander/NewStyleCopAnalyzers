// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.MaintainabilityRules;
    using Xunit;

    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1404CodeAnalysisSuppressionMustHaveJustification,
        StyleCop.Analyzers.MaintainabilityRules.SA1404CodeFixProvider>;

    public partial class SA1404CSharp10UnitTests
    {
        [Fact]
        public async Task TestJustificationWithConstantInterpolatedStringAsync()
        {
            var testCode = @"public class Foo
{
    private const string Reason = ""a reason"";

    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, Justification = $""Because of {Reason}"")]
    public void Bar()
    {
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestJustificationWithEmptyInterpolatedStringAsync()
        {
            var testCode = @"public class Foo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, {|#0:Justification = $""""|})]
    public void Bar()
    {
    }
}";

            var fixedCode = @"public class Foo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, {|#0:Justification = """ + SA1404CodeAnalysisSuppressionMustHaveJustification.JustificationPlaceholder + @"""|})]
    public void Bar()
    {
    }
}";

            await new CSharpTest
            {
                TestCode = testCode,
                ExpectedDiagnostics = { Diagnostic().WithLocation(0) },
                FixedCode = fixedCode,
                RemainingDiagnostics = { Diagnostic().WithLocation(0) },
                NumberOfIncrementalIterations = 2,
                NumberOfFixAllIterations = 2,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }
    }
}
