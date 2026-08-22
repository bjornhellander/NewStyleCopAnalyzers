// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1124DoNotUseRegions>;

    public partial class SA1124CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionWithRegionAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
[|#region Foo|]
    public static void TestMethod()
    {
    }
#endregion
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionWithoutRegionAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
