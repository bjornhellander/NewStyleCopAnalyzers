// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1131UseReadableConditions,
        StyleCop.Analyzers.ReadabilityRules.SA1131CodeFixProvider>;

    public partial class SA1131CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionMethodConstantOnLeftHandSideAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        int i = 5;
        if ([|5 == i|])
        {
        }
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        int i = 5;
        if (i == 5)
        {
        }
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
