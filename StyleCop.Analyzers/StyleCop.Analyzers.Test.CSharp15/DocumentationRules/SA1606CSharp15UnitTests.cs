// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.DocumentationRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1606ElementDocumentationMustHaveSummaryText>;

    public partial class SA1606CSharp15UnitTests : SA1606CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionWithEmptySummaryAsync()
        {
            var testCode = @"
/// <summary>
/// </summary>
public union
[|TestUnion|](string, int);";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionWithSummaryTextAsync()
        {
            var testCode = @"
/// <summary>
/// A test union.
/// </summary>
public union TestUnion(string, int);";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
