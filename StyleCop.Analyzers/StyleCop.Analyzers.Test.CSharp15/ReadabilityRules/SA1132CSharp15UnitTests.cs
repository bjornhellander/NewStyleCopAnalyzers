// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1132DoNotCombineFields,
        StyleCop.Analyzers.ReadabilityRules.SA1132CodeFixProvider>;

    public partial class SA1132CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionCombinedFieldsAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    [|private static int a, b;|]
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    private static int a;
    private static int b;
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
