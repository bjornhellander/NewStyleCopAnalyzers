// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1507CodeMustNotContainMultipleBlankLinesInARow,
        StyleCop.Analyzers.LayoutRules.SA1507CodeFixProvider>;

    public partial class SA1507CSharp10UnitTests
    {
        [Fact]
        public async Task TestConsecutiveMappedLineDirectivesDoNotCountAsBlankLinesAsync()
        {
            var testCode = @"class TestClass
{
#line (5,1)-(5,1) ""Remapped.cs""
#line default
    void Test()
    {
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
