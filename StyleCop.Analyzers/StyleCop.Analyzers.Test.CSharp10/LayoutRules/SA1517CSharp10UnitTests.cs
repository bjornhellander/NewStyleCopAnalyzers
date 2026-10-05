// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1517CodeMustNotContainBlankLinesAtStartOfFile,
        StyleCop.Analyzers.LayoutRules.SA1517CodeFixProvider>;

    public partial class SA1517CSharp10UnitTests
    {
        [Fact]
        public async Task TestMappedLineDirectiveAtFileStartDoesNotTriggerAsync()
        {
            var testCode = @"#line (1,1)-(1,1) ""Remapped.cs""

class TestClass
{
}
#line default";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
