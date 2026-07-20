// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.LayoutRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1516ElementsMustBeSeparatedByBlankLine,
        StyleCop.Analyzers.LayoutRules.SA1516CodeFixProvider>;

    public partial class SA1516CSharp15UnitTests : SA1516CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionMembersNotSeparatedByBlankLineAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public string DisplayName => """";
    public int Count => 0;
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public string DisplayName => """";

    public int Count => 0;
}
";

            var expected = Diagnostic().WithSpan(5, 1, 5, 5);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionMembersSeparatedByBlankLineAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public string DisplayName => """";

    public int Count => 0;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
