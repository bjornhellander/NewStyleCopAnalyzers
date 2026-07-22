// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.DocumentationRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1610PropertyDocumentationMustHaveValueText>;

    public partial class SA1610CSharp15UnitTests : SA1610CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionPropertyWithEmptyValueTagAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    /// <value>
    ///
    /// </value>
    public static int {|#0:TestProperty|}
    {
        get { return 42; }
    }
}
";

            // TODO: Report bug - The compiler calls the registered property declaration action three times
            var expectedDiagnostic = Diagnostic().WithLocation(0);
            DiagnosticResult[] expected = { expectedDiagnostic, expectedDiagnostic, expectedDiagnostic };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionPropertyWithValueTextAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    /// <value>
    /// A value.
    /// </value>
    public static int TestProperty
    {
        get { return 42; }
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
