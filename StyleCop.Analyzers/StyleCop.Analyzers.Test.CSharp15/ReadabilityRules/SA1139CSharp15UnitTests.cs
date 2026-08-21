// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1139UseLiteralSuffixNotationInsteadOfCasting,
        StyleCop.Analyzers.ReadabilityRules.SA1139CodeFixProvider>;

    public partial class SA1139CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionMethodCastInsteadOfLiteralSuffixAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        var x = [|(long)5|];
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        var x = 5L;
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
