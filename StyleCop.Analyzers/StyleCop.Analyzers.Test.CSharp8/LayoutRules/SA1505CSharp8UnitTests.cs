// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1505OpeningBracesMustNotBeFollowedByBlankLine,
        StyleCop.Analyzers.LayoutRules.SA1505CodeFixProvider>;

    public partial class SA1505CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that a blank line between an opening brace and a nullable directive, which C# 8 introduced, is
        /// reported.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestBlankLineBeforeNullableDirectiveAsync()
        {
            var testCode = @"public class TestClass
[|{|]

#nullable enable
    public void Method()
    {
    }
}
";

            var fixedCode = @"public class TestClass
{
#nullable enable
    public void Method()
    {
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
