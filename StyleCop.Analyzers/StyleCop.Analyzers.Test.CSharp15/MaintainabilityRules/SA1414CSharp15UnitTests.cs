// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1414TupleTypesInSignaturesShouldHaveElementNames>;

    public partial class SA1414CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionMethodTupleReturnTypeMissingElementNameAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static (int First, [|int|]) TestMethod() => (1, 2);
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionMethodTupleReturnTypeWithElementNamesAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static (int First, int Second) TestMethod() => (1, 2);
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
