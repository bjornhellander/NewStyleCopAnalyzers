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
        StyleCop.Analyzers.DocumentationRules.SA1608ElementDocumentationMustNotHaveDefaultSummary>;

    public partial class SA1608CSharp15UnitTests : SA1608CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionWithDefaultSummaryAsync()
        {
            var testCode = @"
/// [|<summary>
/// Summary description for the TestUnion union.
/// </summary>|]
public union
TestUnion(string, int);";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionWithoutDefaultSummaryAsync()
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
