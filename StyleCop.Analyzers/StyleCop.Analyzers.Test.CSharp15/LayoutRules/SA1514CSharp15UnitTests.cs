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
        StyleCop.Analyzers.LayoutRules.SA1514ElementDocumentationHeaderMustBePrecededByBlankLine,
        StyleCop.Analyzers.LayoutRules.SA1514CodeFixProvider>;

    public partial class SA1514CSharp15UnitTests : SA1514CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionDocumentationHeaderNotPrecededByBlankLineAsync()
        {
            var testCode = @"
public union First(string, int)
{
}
/// <summary>
/// A test union.
/// </summary>
public union TestUnion(string, int)
{
}
";

            var fixedCode = @"
public union First(string, int)
{
}

/// <summary>
/// A test union.
/// </summary>
public union TestUnion(string, int)
{
}
";

            var expected = Diagnostic().WithSpan(5, 1, 5, 4);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionDocumentationHeaderPrecededByBlankLineAsync()
        {
            var testCode = @"
public union First(string, int)
{
}

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
