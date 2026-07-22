// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1133DoNotCombineAttributes>;

    public partial class SA1133CSharp15UnitTests : SA1133CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionMethodCombinedAttributesAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    [System.Obsolete, {|#0:System.CLSCompliant|}(true)]
    public static void TestMethod()
    {
    }
}
";

            // TODO: Report bug - The compiler calls the registered attribute list action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionMethodSeparateAttributesAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    [System.Obsolete]
    [System.CLSCompliant(true)]
    public static void TestMethod()
    {
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
