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
        StyleCop.Analyzers.LayoutRules.SA1508ClosingBracesMustNotBePrecededByBlankLine,
        StyleCop.Analyzers.LayoutRules.SA1508CodeFixProvider>;

    public partial class SA1508CSharp15UnitTests : SA1508CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionClosingBracePrecededByBlankLineAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public string DisplayName => """";

[|}|]
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public string DisplayName => """";
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionClosingBraceNotPrecededByBlankLineAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public string DisplayName => """";
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
