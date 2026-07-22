// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.DocumentationRules;
    using Xunit;

    public partial class SA1607CSharp15UnitTests : SA1607CSharp14UnitTests
    {
        [Fact]
        public async Task TestPartialUnionWithEmptySummaryAsync()
        {
            var testCode = @"
/// <summary>
/// </summary>
public partial union
[|TestUnion|](string, int);";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestPartialUnionWithSummaryTextAsync()
        {
            var testCode = @"
/// <summary>
/// A test union.
/// </summary>
public partial union TestUnion(string, int);";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
