// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1610PropertyDocumentationMustHaveValueText>;

    public partial class SA1610CSharp15UnitTests
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
    public static int [|TestProperty|]
    {
        get { return 42; }
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
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
