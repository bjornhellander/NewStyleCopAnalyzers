// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.SA1003SymbolsMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1003SymbolsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.SA1003CodeFixProvider>;

    public partial class SA1003CSharp14UnitTests
    {
        [Fact]
        public async Task TestFieldKeywordBinaryExpressionAsync()
        {
            var testCode = @"
public class TestClass
{
    public int Prop
    {
        get => field{|#0:+|}field;
    }
}
";

            var fixedCode = @"
public class TestClass
{
    public int Prop
    {
        get => field + field;
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic(DescriptorPrecededByWhitespace).WithLocation(0).WithArguments("+"),
                Diagnostic(DescriptorFollowedByWhitespace).WithLocation(0).WithArguments("+"),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
