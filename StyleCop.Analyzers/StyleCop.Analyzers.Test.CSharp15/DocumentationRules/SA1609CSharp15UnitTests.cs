// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.DocumentationRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1609PropertyDocumentationMustHaveValue,
        StyleCop.Analyzers.DocumentationRules.SA1609SA1610CodeFixProvider>;

    public partial class SA1609CSharp15UnitTests : SA1609CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionPropertyWithoutValueTagAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// A summary.
    /// </summary>
    public static int {|#0:TestProperty|}
    {
        get { return 42; }
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// A summary.
    /// </summary>
    /// <value>
    /// <placeholder>A summary.</placeholder>
    /// </value>
    public static int TestProperty
    {
        get { return 42; }
    }
}
";

            // TODO: Report bug - The compiler calls the registered property declaration action three times
            var expectedDiagnostic = Diagnostic().WithLocation(0);
            DiagnosticResult[] expected = { expectedDiagnostic, expectedDiagnostic, expectedDiagnostic };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
