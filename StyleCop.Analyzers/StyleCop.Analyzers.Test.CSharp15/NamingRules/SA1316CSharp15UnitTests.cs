// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1316TupleElementNamesShouldUseCorrectCasing,
        StyleCop.Analyzers.NamingRules.SA1316CodeFixProvider>;

    public partial class SA1316CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionTupleElementNameStartingWithLowerCaseLetterAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static (int [|value|], int Other) TestMethod() => (1, 2);
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static (int Value, int Other) TestMethod() => (1, 2);
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
