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
        StyleCop.Analyzers.LayoutRules.SA1505OpeningBracesMustNotBeFollowedByBlankLine,
        StyleCop.Analyzers.LayoutRules.SA1505CodeFixProvider>;

    public partial class SA1505CSharp15UnitTests : SA1505CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionOpeningBraceFollowedByBlankLineAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
[|{|]

    public string DisplayName => """";
}
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
        public async Task TestUnionOpeningBraceNotFollowedByBlankLineAsync()
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
