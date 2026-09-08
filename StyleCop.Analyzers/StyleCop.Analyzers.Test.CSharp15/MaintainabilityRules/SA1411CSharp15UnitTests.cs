// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1411AttributeConstructorMustNotUseUnnecessaryParenthesis,
        StyleCop.Analyzers.MaintainabilityRules.SA1410SA1411CodeFixProvider>;

    public partial class SA1411CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionMethodAttributeWithUnnecessaryParenthesisAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    [System.Obsolete[|()|]]
    public static void TestMethod()
    {
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    [System.Obsolete]
    public static void TestMethod()
    {
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
