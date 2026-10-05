// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1509OpeningBracesMustNotBePrecededByBlankLine,
        StyleCop.Analyzers.LayoutRules.SA1509CodeFixProvider>;

    public partial class SA1509CSharp10UnitTests
    {
        [Fact]
        public async Task TestMappedLineDirectiveBeforeOpeningBraceAsync()
        {
            var testCode = @"class TestClass
{
    private void DoWork()
    {
    }

    public void Test()
    {
#line (10,1)-(10,1) ""Remapped.cs""
        {
            DoWork();
        }
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
