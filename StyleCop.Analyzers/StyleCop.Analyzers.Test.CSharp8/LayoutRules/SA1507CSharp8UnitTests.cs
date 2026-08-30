// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1507CodeMustNotContainMultipleBlankLinesInARow,
        StyleCop.Analyzers.LayoutRules.SA1507CodeFixProvider>;

    public partial class SA1507CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that multiple blank lines before a nullable directive, which C# 8 introduced, are reported.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestMultipleBlankLinesBeforeNullableDirectiveAsync()
        {
            var testCode = @"public class TestClass
{
    public void First()
    {
    }
[|


|]#nullable enable
    public void Second()
    {
    }
}
";

            var fixedCode = @"public class TestClass
{
    public void First()
    {
    }

#nullable enable
    public void Second()
    {
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
