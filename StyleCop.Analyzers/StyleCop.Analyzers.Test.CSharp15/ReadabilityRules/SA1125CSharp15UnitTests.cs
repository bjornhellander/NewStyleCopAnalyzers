// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1125UseShorthandForNullableTypes>;

    public partial class SA1125CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionFieldWithLongFormNullableTypeAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static [|System.Nullable<int>|] field;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionFieldWithShorthandNullableTypeAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static int? field;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
