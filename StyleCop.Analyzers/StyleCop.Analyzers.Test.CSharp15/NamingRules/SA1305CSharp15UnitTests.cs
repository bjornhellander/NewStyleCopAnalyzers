// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.NamingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.NamingRules.SA1305FieldNamesMustNotUseHungarianNotation>;

    public partial class SA1305CSharp15UnitTests : SA1305CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionFieldUsingHungarianNotationAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static int {|#0:nCount|};
}
";

            // TODO: Report bug - The compiler calls the registered variable declaration action three times
            var expected = new[]
            {
                Diagnostic().WithLocation(0).WithArguments("field", "nCount"),
                Diagnostic().WithLocation(0).WithArguments("field", "nCount"),
                Diagnostic().WithLocation(0).WithArguments("field", "nCount"),
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionFieldNotUsingHungarianNotationAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static int count;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
