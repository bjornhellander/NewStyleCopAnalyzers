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
        StyleCop.Analyzers.DocumentationRules.SA1604ElementDocumentationMustHaveSummary>;

    public partial class SA1604CSharp15UnitTests : SA1604CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionWithoutSummaryTextAsync()
        {
            var testCode = @"
///
public union
TestUnion(string, int);";

            DiagnosticResult expected = Diagnostic().WithLocation(4, 1);

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
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
