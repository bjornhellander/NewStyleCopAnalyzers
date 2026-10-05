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
        StyleCop.Analyzers.ReadabilityRules.SA1104SA1105CodeFixProvider>;

    public partial class SA1104CSharp10UnitTests
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
            group value by value;
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
