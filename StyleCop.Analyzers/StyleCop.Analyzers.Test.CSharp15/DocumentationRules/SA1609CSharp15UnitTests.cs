// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1609PropertyDocumentationMustHaveValue,
        StyleCop.Analyzers.DocumentationRules.SA1609SA1610CodeFixProvider>;

    public partial class SA1609CSharp15UnitTests
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
    public static int [|TestProperty|]
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

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
