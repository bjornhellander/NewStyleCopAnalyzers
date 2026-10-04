// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1013ClosingBracesMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1013CSharp9UnitTests
    {
        [Fact]
        public async Task TestWithExpressionAsync()
        {
            var testCode = @"
public record R(int X)
{
    public R M() => this with { X = 1{|#0:}|};
}
";

            var fixedCode = @"
public record R(int X)
{
    public R M() => this with { X = 1 };
}
";

            // SA1013: Closing brace should be preceded by a space
            DiagnosticResult expected = Diagnostic().WithLocation(0).WithArguments(string.Empty, "preceded");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
