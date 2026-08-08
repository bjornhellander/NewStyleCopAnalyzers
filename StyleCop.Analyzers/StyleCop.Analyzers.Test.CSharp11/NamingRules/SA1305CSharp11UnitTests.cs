// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.NamingRules.SA1305FieldNamesMustNotUseHungarianNotation>;

    public partial class SA1305CSharp11UnitTests
    {
        [Fact]
        public async Task TestScopedRefLocalWithHungarianNotationAsync()
        {
            var testCode = @"
public class TestClass
{
    public void Bar()
    {
        int value = 5;
        scoped ref int {|#0:baR|} = ref value;
    }
}
";

            var expected = Diagnostic().WithArguments("variable", "baR").WithLocation(0);

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
