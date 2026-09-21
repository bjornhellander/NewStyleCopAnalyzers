// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1314TypeParameterNamesMustBeginWithT,
        StyleCop.Analyzers.NamingRules.SA1314CodeFixProvider>;

    public partial class SA1314CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionMethodTypeParameterNotStartingWithTAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod<[|X|]>()
    {
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod<TX>()
    {
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
