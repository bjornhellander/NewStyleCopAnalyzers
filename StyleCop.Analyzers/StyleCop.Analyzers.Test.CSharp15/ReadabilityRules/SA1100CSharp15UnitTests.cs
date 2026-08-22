// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1100DoNotPrefixCallsWithBaseUnlessLocalImplementationExists,
        StyleCop.Analyzers.ReadabilityRules.SA1100CodeFixProvider>;

    public partial class SA1100CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionBaseCallWithoutLocalOverrideAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public string TestMethod()
    {
        return [|base|].ToString();
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public string TestMethod()
    {
        return this.ToString();
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
