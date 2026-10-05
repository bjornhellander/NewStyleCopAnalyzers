// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA110xQueryClauses,
        StyleCop.Analyzers.ReadabilityRules.SA1102CodeFixProvider>;

    public partial class SA1102CSharp10UnitTests
    {
        [Fact]
        public async Task TestMappedLineDirectiveBetweenClausesAsync()
        {
            var testCode = @"using System.Linq;
public class TestClass
{
    public void Test()
    {
        var query =
            from value in new int[0]
#line (10,1)-(10,1) ""Remapped.cs""
            where value > 0
            select value;
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
