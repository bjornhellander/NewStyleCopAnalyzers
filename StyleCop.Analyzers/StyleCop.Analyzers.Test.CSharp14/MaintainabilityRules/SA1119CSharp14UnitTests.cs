// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.MaintainabilityRules.SA1119StatementMustNotUseUnnecessaryParenthesis;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1119StatementMustNotUseUnnecessaryParenthesis,
        StyleCop.Analyzers.MaintainabilityRules.SA1119CodeFixProvider>;

    public partial class SA1119CSharp14UnitTests
    {
        [Fact]
        public async Task TestFieldKeywordUnnecessaryParenthesisAsync()
        {
            var testCode = @"
public class TestClass
{
    private int result;

    public int Prop
    {
        get => field;
        set
        {
            field = value;
            this.result = {|#2:{|#0:(|}field{|#1:)|}|};
        }
    }
}
";

            var fixedCode = @"
public class TestClass
{
    private int result;

    public int Prop
    {
        get => field;
        set
        {
            field = value;
            this.result = field;
        }
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic(DiagnosticId).WithLocation(2),
                Diagnostic(ParenthesesDiagnosticId).WithLocation(0),
                Diagnostic(ParenthesesDiagnosticId).WithLocation(1),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
