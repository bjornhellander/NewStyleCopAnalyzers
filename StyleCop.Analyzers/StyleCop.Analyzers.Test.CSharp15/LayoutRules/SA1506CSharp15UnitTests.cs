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
        StyleCop.Analyzers.LayoutRules.SA1506ElementDocumentationHeadersMustNotBeFollowedByBlankLine,
        StyleCop.Analyzers.LayoutRules.SA1506CodeFixProvider>;

    public partial class SA1506CSharp15UnitTests : SA1506CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionDocumentationHeaderFollowedByBlankLineAsync()
        {
            var testCode = @"
/// <summary>
/// A test union.
/// </summary>

public union TestUnion(string, int)
{
}
";

            var fixedCode = @"
/// <summary>
/// A test union.
/// </summary>
public union TestUnion(string, int)
{
}
";

            var expected = Diagnostic().WithSpan(5, 1, 6, 1);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionDocumentationHeaderNotFollowedByBlankLineAsync()
        {
            var testCode = @"
/// <summary>
/// A test union.
/// </summary>
public union TestUnion(string, int)
{
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
